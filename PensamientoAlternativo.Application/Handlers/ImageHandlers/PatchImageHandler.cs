using MediatR;
using PensamientoAlternativo.Application.Commands.ImageCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.ImageHandlers
{
    public sealed class PatchImageHandler : IRequestHandler<PatchImageCommand, bool>
    {
        private readonly IImageWriteRepository _repo;
        private readonly IImageStorage _storage;

        public PatchImageHandler(IImageWriteRepository repo, IImageStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<bool> Handle(PatchImageCommand req, CancellationToken ct)
        {
            var img = await _repo.GetByIdAsync(req.Id, ct);
            if (img is null) return false;

            // Guarda la URL vieja por si se reemplaza el archivo
            var oldUrl = img.Url;

            // 1) Si viene archivo nuevo: subir y obtener nueva URL pública
            string? newPublicUrl = null;
            if (req.Content is not null)
            {
                if (string.IsNullOrWhiteSpace(req.ContentType))
                    throw new ArgumentException("ContentType requerido cuando se envía archivo.");

                // Derivar carpeta a partir del objeto actual (si podemos) para mantener orden;
                // si no, usa 'fotos' / 'banners' según el flag actual* o futuro (preferimos el nuevo si viene).
                var currentObjectName = _storage.TryGetObjectNameFromPublicUrl(oldUrl);
                var folder = "fotos";
                if (req.IsBannerImage ?? img.IsBannerImage) folder = "banners";

                if (!string.IsNullOrWhiteSpace(currentObjectName))
                {
                    var lastSlash = currentObjectName!.LastIndexOf('/');
                    if (lastSlash > 0) folder = currentObjectName[..lastSlash]; // conserva carpeta base previa
                }

                var (baseName, ext) = SplitNameAndExt(req.OriginalFileName ?? "");
                if (string.IsNullOrWhiteSpace(ext))
                    ext = GuessExt(req.ContentType);
                var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title! : baseName);
                var unique = Guid.NewGuid().ToString("N");
                var objectName = $"{folder}/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}{ext}".Trim('/');

                var (_, publicUrl, _) = await _storage.UploadAsync(
                    content: req.Content,
                    contentType: req.ContentType!,
                    objectName: objectName,
                    ct: ct);

                newPublicUrl = publicUrl;
            }

            // 2) Actualizar metadatos (y path si hubo archivo)
            img.UpdateMetadata(req.Title, req.Description, req.IsBannerImage, req.IsActive);
            if (newPublicUrl is not null) img.SetPath(newPublicUrl);

            await _repo.UpdateAsync(img, ct);

            // 3) Si hubo archivo nuevo, intentar borrar el anterior del bucket (best-effort)
            if (newPublicUrl is not null)
            {
                var oldObj = _storage.TryGetObjectNameFromPublicUrl(oldUrl);
                if (oldObj is not null)
                {
                    try { await _storage.DeleteAsync(oldObj, ct); } catch { /* log si quieres */ }
                }
            }

            return true;
        }

        private static (string name, string ext) SplitNameAndExt(string originalFileName)
        {
            var file = originalFileName ?? "image";
            var dot = file.LastIndexOf('.');
            return (dot <= 0 || dot == file.Length - 1) ? (file, "") : (file[..dot], file[dot..].ToLowerInvariant());
        }

        private static string Slugify(string input)
        {
            input ??= string.Empty;
            var s = input.Trim().ToLowerInvariant();
            s = Regex.Replace(s, @"\s+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            return string.IsNullOrWhiteSpace(s) ? "image" : s;
        }

        private static string GuessExt(string contentType) => contentType.ToLowerInvariant() switch
        {
            "image/jpeg" or "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            "image/gif" => ".gif",
            _ => ""  // si no sabemos, omitimos
        };
    }
}
