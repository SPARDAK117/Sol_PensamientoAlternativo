using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Querys;

namespace API_PensamientoAlternativo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ViewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{view}")]
        public async Task<IActionResult> GetViewData(string view)
        {
            Object result;
            if (view == "RompeTusLimites")
            {
                result = await _mediator.Send(new GetBreakYourLimitsViewQuery(view));
                return Ok(result);
            }
            if (view == "Blog")
            {
                result = await _mediator.Send(new GetBlogViewQuery());
                return Ok(result);
            }


            result = await _mediator.Send(new GetViewContentQuery(view));
            return Ok(result);
        }
    }
}
