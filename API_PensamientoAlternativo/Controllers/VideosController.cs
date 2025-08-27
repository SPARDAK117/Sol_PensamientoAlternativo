using Domain.Seedwork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Commands.VideoCommands;
using PensamientoAlternativo.Application.DTOs.VideoDTOs;
using PensamientoAlternativo.Domain.Entities.Sections;

namespace API_PensamientoAlternativo.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/videos")]
    public class VideosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Video> _readRepo;

        public VideosController(IMediator mediator, IRepository<Video> readRepo)
        {
            _mediator = mediator;
            _readRepo = readRepo;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllVideoPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var total = await _readRepo.CountAsync(ct);
            var items = await _readRepo.GetPageAsync(page, pageSize, ct);
            var list = items.Select(v => new { v.Id, v.Title, v.Description, v.Url, v.IsVisible,v.ViewSection });
            return Ok(new { items = list, total, page, pageSize });
        }

        [HttpPost("createVideoPost")]
        [DisableRequestSizeLimit]
        [RequestSizeLimit(600_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 600_000_000)]
        public async Task<IActionResult> CreateVideoPost([FromForm] CreateVideoFormDto form, CancellationToken ct)
        {
            if (form.File is null || form.File.Length == 0)
                return BadRequest("Debe enviar un archivo en 'File'.");
            if (!form.File.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos de video.");

            await using var stream = form.File is null
                ? null
                : form.File.OpenReadStream();


            var id = await _mediator.Send(new CreateVideoCommand(
                form.Title,
                form.Description,
                form.IsVisible,
                form.ViewSection,
                stream,
                form.File.ContentType,
                form.File.FileName
            ), ct);

            return Ok(new { id });
        }

        [HttpPatch("patchVideoPostbyId/{id:int}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> PatchVideoPost([FromRoute] int id, [FromForm] UpdateVideoFormDto form, CancellationToken ct)
        {
            await using var stream = form.File is null ? null : form.File.OpenReadStream();
            if (form.File is not null && !form.File.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten archivos de video.");

            var ok = await _mediator.Send(new PatchVideoCommand(
                id,
                form.Title,
                form.Description,
                form.IsVisible,
                stream,
                form.File?.ContentType,
                form.File?.FileName
            ), ct);

            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("deleteVideoPostbyId/{id:int}")]
        public async Task<IActionResult> DeleteVideoPost([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _mediator.Send(new DeleteVideoCommand(id), ct);
            return ok ? NoContent() : NotFound();
        }
    }

}
