using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Interfaces
{
    public interface IImageStorage
    {
        /// <summary>
        /// Sube un objeto al bucket y devuelve (storagePath, publicUrl).
        /// storagePath: "gs://bucket/objName"
        /// publicUrl:   "https://storage.googleapis.com/bucket/objName"
        /// </summary>
        Task<(string storagePath, string publicUrl, string objectName)> UploadAsync(
            Stream content,
            string contentType,
            string objectName,
            CancellationToken ct);
        Task DeleteAsync(string objectName, CancellationToken ct);

        // Útil si solo guardaste la URL pública en DB:
        Task DeleteByPublicUrlAsync(string publicUrl, CancellationToken ct);

        // Si quieres usarlo desde el handler para inspección/logs:
        string? TryGetObjectNameFromPublicUrl(string publicUrl);
    }

}
