using Domain.Seedwork;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Application.Commands.FaqCommands;
using PensamientoAlternativo.Application.Commands.ImageCommands;
using PensamientoAlternativo.Application.Commands.OpinionsCommands;
using PensamientoAlternativo.Application.DTOs.CategoriesDTOs;
using PensamientoAlternativo.Application.DTOs.FaqDTOs;
using PensamientoAlternativo.Application.DTOs.ImageDTOs;
using PensamientoAlternativo.Application.DTOs.OpinionsDTOs;
using PensamientoAlternativo.Domain.Entities.Blog;
using PensamientoAlternativo.Domain.Entities.Sections;

namespace API_PensamientoAlternativo.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContentManagmentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Faq> _Faqrepo;
        private readonly IRepository<Image> _Imagerepo;
        private readonly IRepository<Opinion> _Opinionrepo;
        private readonly IRepository<BlogCategory> _BlogCategoryrepo;
        public ContentManagmentController(IMediator mediator,
                                          IRepository<Faq> faqRepo,
                                          IRepository<Image> imgRepo,
                                          IRepository<Opinion> opinionRepo,
                                          IRepository<BlogCategory> blogCategoryRepo)
        {
            _mediator = mediator;
            _Faqrepo = faqRepo;
            _Imagerepo = imgRepo;
            _Opinionrepo = opinionRepo;
            _BlogCategoryrepo = blogCategoryRepo;
        }

        [HttpGet("Images/getAll")]
        public async Task<IActionResult> GetAllImages(CancellationToken ct)
        {
            var items = await _Imagerepo.GetAllAsync(ct);
            var list = items.Select(i => new {
                i.Id,
                i.Title,
                Url = i.Path,
                i.IsBannerImage,
                i.IsVisible
            });
            return Ok(list);
        }

        [HttpPost("Images/createImage")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> Create([FromForm] CreateImageFormDto form, CancellationToken ct)
        {
            if (form.File is null || form.File.Length == 0)
                return BadRequest("Debe enviar un archivo en 'File'.");

            if (form.File.Length > 20_000_000) return BadRequest("El archivo excede 20MB.");
            if (!form.File.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Solo se permiten imágenes.");

            await using var stream = form.File.OpenReadStream();

            var id = await _mediator.Send(new CreateImageCommand(
                form.IsBannerImage,
                form.IsActive,
                form.ViewSection,
                form.Title,
                form.Description,
                stream,
                form.File.ContentType,
                form.File.FileName
            ), ct);

            return Ok(new { id });
        }

        [HttpPatch("Images/patchImagebyId/{id:int}")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> PatchImages([FromRoute] int id, [FromForm] UpdateImageFormDto form, CancellationToken ct)
        {
            await using var stream = form.File is null ? null : form.File.OpenReadStream();

            if (form.File is not null)
            {
                if (form.File.Length > 20_000_000) return BadRequest("El archivo excede 20MB.");
                if (!form.File.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Solo se permiten imágenes.");
            }

            var ok = await _mediator.Send(new PatchImageCommand(
                id,
                form.Title,
                form.Description,
                form.IsBannerImage,
                form.IsActive,
                stream,                          
                form.File?.ContentType,
                form.File?.FileName
            ), ct);

            return ok ? NoContent() : NotFound();
        }
        [HttpDelete("Images/deleteImage/{id:int}")]
        public async Task<IActionResult> DeleteImage([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _mediator.Send(new DeleteImageCommand(id), ct);
            return ok ? NoContent() : NotFound();
        }
        [HttpGet("Faq/getAll")]
        public async Task<IActionResult> GetAllFaqs(CancellationToken ct)
        {
            var items = await _Faqrepo.GetAllAsync(ct);
            var list = items.Select(f => new { f.Id, f.Question, f.Answer });
            return Ok(list);
        }

        [HttpPost("Faq/createfaq")]
        public async Task<IActionResult> CreateFaq([FromBody] CreateFaqRequest body, CancellationToken ct)
        {
            var id = await _mediator.Send(new CreateFaqCommand(body.Question, body.Answer,body.IsVisible), ct);
            return Ok(new { id });
        }

        [HttpPatch("Faq/patchFaqById{id:int}")]
        public async Task<IActionResult> PatchFaq([FromRoute] int id, [FromBody] UpdateFaqRequest body, CancellationToken ct)
        {
            var ok = await _mediator.Send(new PatchFaqCommand(id, body.Question, body.Answer, body.IsVisible), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("Faq/deleteFaq/{id:int}")]
        public async Task<IActionResult> DeleteFaq([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _mediator.Send(new DeleteFaqCommand(id), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("Opinions/getAll")]
        public async Task<IActionResult> GetAllOpinions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
        {
            var total = await _Opinionrepo.CountAsync(ct);
            var items = await _Opinionrepo.GetPageAsync(page, pageSize, ct);

            var list = items.Select(o => new {
                o.Id,
                o.AuthorName,
                o.StarRate,
                o.OpinionText,
                o.CreatedDate
            });

            return Ok(new
            {
                items = list,
                total,
                page,
                pageSize
            });
        }

        [HttpPost("Opinions/createOpinion")]
        public async Task<IActionResult> CreateOpinion([FromBody] CreateOpinionRequest body, CancellationToken ct)
        {
            var id = await _mediator.Send(new CreateOpinionCommand(body.AuthorName, body.StarRate, body.OpinionText,body.IsVisible), ct);
            return Ok(new { id }); // o 201 si prefieres
        }

        [HttpPatch("Opinions/patchOpinionbyId/{id:int}")]
        public async Task<IActionResult> PatchOpinionById([FromRoute] int id, [FromBody] UpdateOpinionRequest body, CancellationToken ct)
        {
            var ok = await _mediator.Send(new PatchOpinionCommand(id, body.AuthorName, body.StarRate, body.OpinionText,body.IsVisible), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("Opinions/deleteOpinion/{id:int}")]
        public async Task<IActionResult> DeleteOpinion([FromRoute] int id, CancellationToken ct)
        {
            var ok = await _mediator.Send(new DeleteOpinionCommand(id), ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("Categories/getAll")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var items = await _BlogCategoryrepo.GetAllAsync(ct);
            var list = items.Select(f => new { f.Id ,f.Name});
            return Ok(list);
        }

        [HttpPost("Categories/createCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateBlogCategoryRequest body, CancellationToken ct)
        {
            try
            {
                var id = await _mediator.Send(new CreateBlogCategoryCommand(body.Name), ct);
                return Ok(new { id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpPatch("Categories/patchCategory{id:int}")]
        public async Task<IActionResult> PatchCategoryById([FromRoute] int id, [FromBody] UpdateBlogCategoryRequest body, CancellationToken ct)
        {
            try
            {
                var ok = await _mediator.Send(new PatchBlogCategoryCommand(id, body.Name), ct);
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        [HttpDelete("Categories/deleteCategory{id:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id, CancellationToken ct)
        {
            try
            {
                var ok = await _mediator.Send(new DeleteBlogCategoryCommand(id), ct);
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}
