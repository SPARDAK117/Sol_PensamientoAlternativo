using MediatR;
using PensamientoAlternativo.Application.Commands.VideoCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.VideoHandlers
{
    public sealed class CreateVideoHandler : IRequestHandler<CreateVideoCommand, int>
    {
        private readonly IVideoWriteRepository _repo;
        private readonly IImageStorage _storage;

        public CreateVideoHandler(IVideoWriteRepository repo, IImageStorage storage)
        {
            _repo = repo;
            _storage = storage;
        }

        public async Task<int> Handle(CreateVideoCommand req, CancellationToken ct)
        {
            if (req.Content is null) throw new ArgumentNullException(nameof(req.Content));
            if (string.IsNullOrWhiteSpace(req.ContentType)) throw new ArgumentException("ContentType requerido.");

            var (name, ext) = SplitNameAndExt(req.OriginalFileName ?? "");
            if (string.IsNullOrWhiteSpace(ext)) ext = GuessVideoExt(req.ContentType);

            var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title : name);
            var unique = Guid.NewGuid().ToString("N");
            var objectName = $"videos/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}{ext}";

            var (_, publicUrl, _) = await _storage.UploadAsync(
                req.Content, req.ContentType, objectName, ct);

            var video = new Video(
                title: req.Title.Trim(),
                description: req.Description.Trim(),
                url: publicUrl,
                isVisible: req.IsVisible
            );

            return await _repo.CreateAsync(video, ct);
        }

        private static (string name, string ext) SplitNameAndExt(string original)
        {
            var dot = original.LastIndexOf('.');
            return (dot <= 0 || dot == original.Length - 1) ? (original, "") : (original[..dot], original[dot..].ToLowerInvariant());
        }

        private static string Slugify(string s)
        {
            s ??= string.Empty;
            s = s.Trim().ToLowerInvariant();
            s = Regex.Replace(s, @"\s+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            return string.IsNullOrWhiteSpace(s) ? "video" : s;
        }

        private static string GuessVideoExt(string contentType) => contentType.ToLowerInvariant() switch
        {
            "video/mp4" => ".mp4",
            "video/webm" => ".webm",
            "video/quicktime" => ".mov",
            "video/x-msvideo" => ".avi",
            "video/x-matroska" => ".mkv",
            _ => "" // si desconocido, sin extensión
        };
    }
}
