using MediatR;
using PensamientoAlternativo.Application.Commands.ImageCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
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
                // Validación mínima
                if (string.IsNullOrWhiteSpace(req.ContentType))
                    throw new ArgumentException("ContentType requerido cuando se envía archivo.");
                if (!req.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("El archivo debe ser de tipo imagen.");

                // Carpeta según banner actual o nuevo
                var folder = (req.IsBannerImage ?? img.IsBannerImage) ? "banners" : "fotos";

                // Nombre base/slug; siempre forzamos extensión .webp
                var (baseName, _) = SplitNameAndExt(req.OriginalFileName ?? "");
                var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title! : baseName);
                var unique = Guid.NewGuid().ToString("N");
                var objectName = $"{folder}/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}.webp".Trim('/');

                // Convertir SIEMPRE a WebP (lossy calidad 80; puedes ajustar)
                await using var webpStream = await ToWebpStreamAsync(req.Content, quality: 80, ct);

                var (_, publicUrl, _) = await _storage.UploadAsync(
                    content: webpStream,
                    contentType: "image/webp", 
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

        private static async Task<MemoryStream> ToWebpStreamAsync(Stream input, int quality, CancellationToken ct)
        {
            using var originalBuffer = new MemoryStream();
            await input.CopyToAsync(originalBuffer, ct);
            originalBuffer.Position = 0;

            using var image = await Image.LoadAsync(originalBuffer, ct);
            image.Mutate(x => x.AutoOrient()); // respeta EXIF (fotos móviles)

            var output = new MemoryStream();
            var encoder = new WebpEncoder
            {
                Quality = quality,                    // 0..100 (80 suele ser buen balance)
                FileFormat = WebpFileFormatType.Lossy // usa Lossless para logos/PNG-like
            };

            await image.SaveAsync(output, encoder, ct);
            output.Position = 0;
            return output;
        }
    }
}
