using MediatR;
using PensamientoAlternativo.Application.Commands.VideoCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.VideoHandlers
{
    public sealed class PatchVideoHandler : IRequestHandler<PatchVideoCommand, bool>
    {
        private readonly IVideoWriteRepository _repo;
        private readonly IImageStorage _storage;

        public PatchVideoHandler(IVideoWriteRepository repo, IImageStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<bool> Handle(PatchVideoCommand req, CancellationToken ct)
        {
            var v = await _repo.GetByIdAsync(req.Id, ct);
            if (v is null) return false;
            string oldUrlDBName = v.Url;
            string? newUrl = null;

            if (req.Content is not null)
            {
                if (string.IsNullOrWhiteSpace(req.ContentType))
                    throw new ArgumentException("ContentType requerido cuando se envía archivo.");

                // Intenta mantener carpeta base si ya había objeto
                var currentObj = _storage.TryGetObjectNameFromPublicUrl(v.Url);
                var folder = "videos";
                //if (!string.IsNullOrWhiteSpace(currentObj))
                //{
                //    var lastSlash = currentObj!.LastIndexOf('/');
                //    if (lastSlash > 0) folder = currentObj[..lastSlash];
                //}

                var (name, ext) = SplitNameAndExt(req.OriginalFileName ?? "");
                if (string.IsNullOrWhiteSpace(ext)) ext = GuessVideoExt(req.ContentType);

                var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title! : name);
                var unique = Guid.NewGuid().ToString("N");
                var objectName = $"{folder}/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}{ext}".Trim('/');

                var (_, publicUrl, _) = await _storage.UploadAsync(
                    req.Content, req.ContentType!, objectName, ct);

                newUrl = publicUrl;
            }

            v.Update(req.Title, req.Description, req.IsVisible);
            if (newUrl is not null) v.SetUrl(newUrl);

            await _repo.UpdateAsync(v, ct);

            if (newUrl is not null)
            {
                var oldObj = _storage.TryGetObjectNameFromPublicUrl(oldUrlDBName);
                if (oldObj is not null)
                {
                    try { await _storage.DeleteAsync(oldObj, ct); } catch { /* log si quieres */ }
                }
            }

            return true;
        }

        // helpers reutilizados…
        private static (string name, string ext) SplitNameAndExt(string original)
        {
            var dot = original.LastIndexOf('.');
            return (dot <= 0 || dot == original.Length - 1) ? (original, "") : (original[..dot], original[dot..].ToLowerInvariant());
        }
        private static string Slugify(string s)
        {
            s ??= string.Empty;
            s = System.Text.RegularExpressions.Regex.Replace(s.Trim().ToLowerInvariant(), @"\s+", "-");
            s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-z0-9\-]", "");
            return string.IsNullOrWhiteSpace(s) ? "video" : s;
        }
        private static string GuessVideoExt(string contentType) => contentType.ToLowerInvariant() switch
        {
            "video/mp4" => ".mp4",
            "video/webm" => ".webm",
            "video/quicktime" => ".mov",
            "video/x-msvideo" => ".avi",
            "video/x-matroska" => ".mkv",
            _ => ""
        };
    }
}
