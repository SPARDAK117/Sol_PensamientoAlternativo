using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using PensamientoAlternativo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Infrastructure.Storage
{
    public sealed class FirebaseImageStorage : IImageStorage
    {
        private readonly StorageClient _client;
        private readonly string _bucket;

        public FirebaseImageStorage(IConfiguration cfg)
        {
            // appsettings.json
            // "Firebase": { "ServiceAccountJsonPath": "C:\\keys\\firebase-sa.json", "Bucket": "pensamiento-alternativo-17.appspot.com" }
            var saPath = cfg["Firebase:ServiceAccountJsonPath"];
            _bucket = cfg["Firebase:Bucket"] ?? throw new InvalidOperationException("Firebase:Bucket requerido.");

            GoogleCredential credential = string.IsNullOrWhiteSpace(saPath)
                ? GoogleCredential.GetApplicationDefault()
                : GoogleCredential.FromFile(saPath);

            _client = StorageClient.Create(credential);
        }

        public async Task<(string storagePath, string publicUrl, string objectName)> 
            UploadAsync(Stream content,
                        string contentType,
                        string objectName,
                        CancellationToken ct)
        {
            // Token de descarga tipo Firebase
            var downloadToken = Guid.NewGuid().ToString();

            // En este overload, ContentType se pone en el Object
            var obj = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = _bucket,
                Name = objectName,
                ContentType = contentType, // <-- AQUÍ
                Metadata = new Dictionary<string, string>
                {
                    ["firebaseStorageDownloadTokens"] = downloadToken
                }
            };

            // No intentes pasar ContentType en UploadObjectOptions: no existe esa propiedad
            await _client.UploadObjectAsync(obj, content, cancellationToken: ct);

            var storagePath = $"gs://{_bucket}/{objectName}";
            var encoded = Uri.EscapeDataString(objectName);
            var publicUrl = $"https://firebasestorage.googleapis.com/v0/b/{_bucket}/o/{encoded}?alt=media&token={downloadToken}";

            return (storagePath, publicUrl, objectName);
        }

        public async Task DeleteAsync(string objectName, CancellationToken ct)
        {
            try
            {
                await _client.DeleteObjectAsync(_bucket, objectName, cancellationToken: ct);
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                // Idempotente: si no existe en el bucket, lo consideramos eliminado
            }
        }

        // ====== NUEVO: DELETE por URL pública ======
        public async Task DeleteByPublicUrlAsync(string publicUrl, CancellationToken ct)
        {
            var objectName = TryGetObjectNameFromPublicUrl(publicUrl)
                ?? throw new InvalidOperationException("No se pudo extraer el objectName desde la URL pública.");
            await DeleteAsync(objectName, ct);
        }

        // ====== NUEVO: helper para extraer objectName desde URLs Firebase/Storage ======
        public string? TryGetObjectNameFromPublicUrl(string publicUrl)
        {
            if (string.IsNullOrWhiteSpace(publicUrl)) return null;
            if (!Uri.TryCreate(publicUrl, UriKind.Absolute, out var uri)) return null;

            // Caso típico Firebase:
            // https://firebasestorage.googleapis.com/v0/b/{bucket}/o/{ENCODED_OBJECT}?alt=media&token=...
            var segs = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            // Esperamos: [v0, b, {bucket}, o, {encoded-object}]
            if (segs.Length >= 5 &&
                segs[0].Equals("v0", StringComparison.OrdinalIgnoreCase) &&
                segs[1].Equals("b", StringComparison.OrdinalIgnoreCase) &&
                segs[3].Equals("o", StringComparison.OrdinalIgnoreCase))
            {
                var encoded = segs[4];
                return Uri.UnescapeDataString(encoded);
            }

            // Alternativa GCS pública:
            // https://storage.googleapis.com/{bucket}/{objectName}
            if (uri.Host.Equals("storage.googleapis.com", StringComparison.OrdinalIgnoreCase))
            {
                var parts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                // Esperamos: [{bucket}, {object...}]
                if (parts.Length >= 2 && string.Equals(parts[0], _bucket, StringComparison.Ordinal))
                {
                    var objectParts = parts.Skip(1);
                    var obj = string.Join('/', objectParts);
                    return Uri.UnescapeDataString(obj);
                }
            }

            return null;
        }
    }

}
