using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Commands;
using PensamientoAlternativo.Application.DTOs.BlogDto;
using PensamientoAlternativo.Application.Interfaces;
using PensamientoAlternativo.Application.Querys;
using PensamientoAlternativo.Domain.Interfaces;
using PensamientoAlternativo.Persistance.Repositories;

namespace API_PensamientoAlternativo.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlogController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet("Articlelist")]
        public async Task<IActionResult> GetArticleList([FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string? category = null)
        {
            var result = await _mediator.Send(new GetBlogArticleListQuery(page, pageSize, category));
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("getArticleById/{id}")]
        public async Task<IActionResult> GetBlogArticleById(int id)
        {
            var result = await _mediator.Send(new GetBlogArticleByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [HttpPost("createArticle")]
        public async Task<IActionResult> CreateBlogArticle([FromBody] CreateArticleRequest body, CancellationToken ct)
        {
            var id = await _mediator.Send(new CreateArticleCommand(
                body.Title,
                body.Description,
                body.ImageUrl,
                body.CategoryId,
                body.ContentHtml
            ), ct);

            return CreatedAtAction(nameof(GetBlogArticleById), new { id }, new { id });
        }
        [HttpPatch("patchArticleById/{id:int}")]
        public async Task<IActionResult> PatchArticle([FromRoute] int id, [FromBody] UpdateArticleRequest body, CancellationToken ct)
        {
            var ok = await _mediator.Send(new PatchArticleCommand(
                id,
                body.Title,
                body.Description,
                body.ImageUrl,
                body.CategoryId,
                body.ContentHtml
            ), ct);

            return ok ? NoContent() : NotFound();
        }
        [HttpDelete("deleteArticleById/{id:int}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id, CancellationToken ct)
        {
            var deleted = await _mediator.Send(new DeleteArticleCommand(id), ct);
            return deleted ? NoContent() : NotFound();
        }
    }
}
