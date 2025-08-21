using MediatR;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Application.Commands.ImageCommands;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PensamientoAlternativo.Application.Handlers.ImageHandlers
{
    public class CreateImageHandler : IRequestHandler<CreateImageCommand, int>
    {
        private readonly IImageWriteRepository _imageRepository;
        private readonly IImageStorage _storage;

        public CreateImageHandler(IImageWriteRepository repo, IImageStorage storage)
        {
            _imageRepository = repo;
            _storage = storage;
        }

        public async Task<int> Handle(CreateImageCommand req, CancellationToken ct)
        {
            if (req.Content is null) throw new ArgumentNullException(nameof(req.Content));
            if (string.IsNullOrWhiteSpace(req.ContentType)) throw new ArgumentException("ContentType requerido", nameof(req.ContentType));
            if (string.IsNullOrWhiteSpace(req.OriginalFileName)) throw new ArgumentException("OriginalFileName requerido", nameof(req.OriginalFileName));

            // 1) Derivar carpeta según reglas del servidor
            var folder = req.IsBannerImage ? "banners" : "fotos";

            // 2) Generar nombre único y limpio
            var (name, ext) = SplitNameAndExt(req.OriginalFileName);
            if (string.IsNullOrWhiteSpace(ext))
                ext = GuessExt(req.ContentType); // opcional: infiere extensión si falta

            var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title : name);
            var unique = Guid.NewGuid().ToString("N"); // evita colisiones
            var objectName = $"{folder}/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}{ext}";

            // 3) Subir a Firebase Storage (crea token y devuelve URL pública)
            var (_, publicUrl, _) = await _storage.UploadAsync(
                content: req.Content,
                contentType: req.ContentType,
                objectName: objectName,
                ct: ct);

            // 4) Guardar en DB la URL pública (lista para el front)
            Image image = new (
                isBannerImage: req.IsBannerImage,
                isVisible: req.IsVisible,
                title: req.Title?.Trim() ?? string.Empty,
                path: publicUrl,
                description: req.Description?.Trim() ?? string.Empty
            );

            return await _imageRepository.CreateAsync(image, ct);
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
            _ => "" // si no sabes, deja sin extensión
        };
    }
}
