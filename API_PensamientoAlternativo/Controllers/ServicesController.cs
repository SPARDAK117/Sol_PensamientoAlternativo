using Domain.Seedwork;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PensamientoAlternativo.Application.DTOs.ServiceDTOs;
using PensamientoAlternativo.Application.DTOs.VideoDTOs;
using PensamientoAlternativo.Domain.Entities.Sections;
using PensamientoAlternativo.Domain.SeedWork;
using System.Runtime.InteropServices;

namespace API_PensamientoAlternativo.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        //private readonly IMediator _mediator;
        private readonly IRepository<Service> _readRepo;
        private readonly IWriteRepository<Service> _writeRepo;

        public ServicesController(IRepository<Service> readRepo,IWriteRepository<Service> writeRepo) 
        {
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllServices([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            int total = await _readRepo.CountAsync(ct);
            List<Service> items = await _readRepo.GetPageAsync(page, pageSize, ct);
            var list = items.Select(v => new { v.Id, v.Title, v.IconPath, v.Subtitle});
            return Ok(new { items = list, total, page, pageSize });

        }

        [HttpPost("createService")]

        public async Task<IActionResult> CreateNewService([FromForm] ServiceRequestDto form, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(form.Title)) return BadRequest("Debe agregar un Title.");
            if (string.IsNullOrEmpty(form.Subtitle)) return BadRequest("Debe agregar un Subtitle.");


            Service newservice = new(form.Title, form.Subtitle, form.IconName, form.IconPath)
            {
                Title = form.Title,
                IconPath = form.IconPath,
                Subtitle = form.Subtitle,
                IconName = form.IconName,
            };

            await _readRepo.AddAsync(newservice);
            await _readRepo.SaveAsync();

            return Ok(newservice.Id);
        }

        [HttpPatch("patchServiceById/{id:int}")]
        public async Task<IActionResult> PatchServiceById([FromRoute] int id, [FromForm] ServiceRequestDto form, CancellationToken ct)
        {

            var oldService = await _readRepo.GetByIdAsync(id);
            if (oldService == null) return NotFound($"No se encontró ningún servicio con el ID {id}");

            oldService.UpdateMetadata(form?.Title ?? oldService.Title, form?.Subtitle ?? oldService.Subtitle, form?.IconPath ?? oldService.IconPath, form?.IconName ?? oldService.IconName);

            await _writeRepo.UpdateAsync(oldService,ct);
            return Ok(new { message = "Servicio actualizado correctamente", serviceId = id });
        }

        [HttpDelete("deleteServiceById/{id:int}")]
        public async Task<IActionResult> DeleteServiceById([FromRoute] int id, CancellationToken ct)
        {
            var serviceToDelete = await _readRepo.GetByIdAsync(id); 

            if (serviceToDelete == null) return NotFound();

            bool ok = await _writeRepo.DeleteAsync(id,ct);
            await _readRepo.SaveAsync();

            return Ok(ok);
        }
    }
}
