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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;


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

            // 2) Nombre único y limpio (usaremos .webp)
            var (name, _) = SplitNameAndExt(req.OriginalFileName);
            var slug = Slugify(!string.IsNullOrWhiteSpace(req.Title) ? req.Title : name);
            var unique = Guid.NewGuid().ToString("N");
            var ext = ".webp"; // <-- forzamos .webp
            var objectName = $"{folder}/{DateTime.UtcNow:yyyy/MM}/{unique}-{slug}{ext}";

            // 3) Convertir SIEMPRE a WebP (lossy con calidad 80)
            await using var webpStream = await ToWebpStreamAsync(req.Content, quality: 80, ct);

            // 4) Subir a Firebase como image/webp
            var (_, publicUrl, _) = await _storage.UploadAsync(
                content: webpStream,
                contentType: "image/webp",          // <-- importante
                objectName: objectName,
                ct: ct);

            // 4) Guardar en DB la URL pública (lista para el front)
            var image = new Domain.Entities.Sections.Image(
                isBannerImage: req.IsBannerImage,
                isVisible: req.IsVisible,
                viewSection: req.ViewSection,
                title: req.Title?.Trim() ?? string.Empty,
                url: publicUrl,
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

        private static string GuessExt(string contentType) => ".webp";

        private static async Task<MemoryStream> ToWebpStreamAsync(Stream input, int quality, CancellationToken ct)
        {
            // Copiamos a memoria en caso de streams no seekeables
            using var originalBuffer = new MemoryStream();
            await input.CopyToAsync(originalBuffer, ct);
            originalBuffer.Position = 0;

            using var image = await Image.LoadAsync(originalBuffer, ct);
            image.Mutate(x => x.AutoOrient()); // respeta EXIF/rotación

            var output = new MemoryStream();
            var encoder = new WebpEncoder
            {
                Quality = quality,                    // 0..100
                FileFormat = WebpFileFormatType.Lossy // Lossy recomendado para fotos; usa Lossless si prefieres PNG-like
            };

            await image.SaveAsync(output, encoder, ct);
            output.Position = 0;
            return output; // caller hace await using
        }
    }
}
