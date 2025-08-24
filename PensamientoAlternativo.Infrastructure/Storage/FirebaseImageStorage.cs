using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google.Apis.Storage.v1;
using Microsoft.Extensions.Configuration;
using PensamientoAlternativo.Application.Interfaces;
using System.Net;

namespace PensamientoAlternativo.Infrastructure.Storage
{
    public sealed class FirebaseImageStorage : IImageStorage
    {
        private readonly StorageClient _client;
        private readonly string _bucket;

        public FirebaseImageStorage(IConfiguration cfg)
        {
            _bucket = cfg["Firebase:Bucket"]
                ?? throw new InvalidOperationException("Firebase:Bucket requerido (ej. 'pensamiento-alternativo-17.appspot.com').");

            var saPath = cfg["Firebase:ServiceAccountJsonPath"];      
            var saJson = cfg["Firebase:ServiceAccountJson"];          
            var saJson64 = cfg["Firebase:ServiceAccountJsonBase64"];   

            GoogleCredential cred;

            if (!string.IsNullOrWhiteSpace(saJson64))
            {
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(saJson64));
                cred = GoogleCredential.FromJson(json);
            }
            else if (!string.IsNullOrWhiteSpace(saJson) && saJson.TrimStart().StartsWith("{"))
            {
                // Si el JSON viene con \n escapados, no pasa nada aquí
                cred = GoogleCredential.FromJson(saJson);
            }
            else if (!string.IsNullOrWhiteSpace(saPath))
            {
                if (Directory.Exists(saPath))
                    throw new InvalidOperationException($"Firebase:ServiceAccountJsonPath apunta a un directorio, no a un archivo: {saPath}");
                if (!File.Exists(saPath))
                    throw new FileNotFoundException($"No se encontró el Service Account en: {saPath}");
                cred = GoogleCredential.FromFile(saPath);
            }
            else
            {
                cred = GoogleCredential.GetApplicationDefault();
            }

            if (cred.IsCreateScopedRequired)
                cred = cred.CreateScoped(StorageService.Scope.DevstorageFullControl);

            _client = StorageClient.Create(cred);
        }

        public async Task<(string storagePath, string publicUrl, string objectName)>
            UploadAsync(Stream content, string contentType, string objectName, CancellationToken ct)
        {
            var downloadToken = Guid.NewGuid().ToString();

            var obj = new Google.Apis.Storage.v1.Data.Object
            {
                Bucket = _bucket,
                Name = objectName,
                ContentType = contentType,
                Metadata = new Dictionary<string, string>
                {
                    ["firebaseStorageDownloadTokens"] = downloadToken
                }
            };

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
                // idempotente: lo consideramos borrado
            }
        }

        public async Task DeleteByPublicUrlAsync(string publicUrl, CancellationToken ct)
        {
            var objectName = TryGetObjectNameFromPublicUrl(publicUrl)
                ?? throw new InvalidOperationException("No se pudo extraer el objectName desde la URL pública.");
            await DeleteAsync(objectName, ct);
        }

        public string? TryGetObjectNameFromPublicUrl(string publicUrl)
        {
            if (string.IsNullOrWhiteSpace(publicUrl)) return null;
            if (!Uri.TryCreate(publicUrl, UriKind.Absolute, out var uri)) return null;

            // Firebase: https://firebasestorage.googleapis.com/v0/b/{bucket}/o/{ENCODED_OBJECT}?...
            var segs = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segs.Length >= 5 &&
                segs[0].Equals("v0", StringComparison.OrdinalIgnoreCase) &&
                segs[1].Equals("b", StringComparison.OrdinalIgnoreCase) &&
                segs[3].Equals("o", StringComparison.OrdinalIgnoreCase))
            {
                var encoded = segs[4];
                return Uri.UnescapeDataString(encoded);
            }

            // GCS público: https://storage.googleapis.com/{bucket}/{object...}
            if (uri.Host.Equals("storage.googleapis.com", StringComparison.OrdinalIgnoreCase))
            {
                var parts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && string.Equals(parts[0], _bucket, StringComparison.Ordinal))
                {
                    var obj = string.Join('/', parts.Skip(1));
                    return Uri.UnescapeDataString(obj);
                }
            }

            return null;
        }
    }
}
