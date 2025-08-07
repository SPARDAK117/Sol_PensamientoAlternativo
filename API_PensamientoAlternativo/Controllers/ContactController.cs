using API_PensamientoAlternativo.DTOs.ContactFormDTOs;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.Commands.FormCommand;
using System.Threading.Tasks;

namespace API_PensamientoAlternativo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewContactForm([FromBody] ContactFormDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) ||
                string.IsNullOrWhiteSpace(dto.Message) ||
                string.IsNullOrWhiteSpace(dto.Email))
            {
                return BadRequest(new { Message = "Name, message, and email are required." });
            }

            var result = await _mediator.Send(new SubmitContactFormCommand
            {
                Name = dto.Name,
                Email = dto.Email,
                Message = dto.Message,
                Phone = dto.Phone
            });

            return Ok(result);
        }
    }
}
