using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Querys;

namespace API_PensamientoAlternativo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlogController(IMediator mediator) 
        {
            _mediator = mediator;
        }
        [HttpGet("Articlelist")]
        public async Task<IActionResult> GetArticleList([FromQuery] int page = 1, [FromQuery] int pageSize = 8, [FromQuery] string? category = null)
        {
            var result = await _mediator.Send(new GetBlogArticleListQuery(page, pageSize, category));
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogArticleById(int id)
        {
            var result = await _mediator.Send(new GetBlogArticleByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
