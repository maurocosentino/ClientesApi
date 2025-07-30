using ClienteApi.Data;
using ClienteApi.Dtos;
using ClienteApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClienteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteDbContext _context;

        public ClienteController(ClienteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var argentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");

            var clientes = await _context.Clientes
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Email = c.Email,
                Telefono = c.Telefono,
                Direccion = c.Direccion,
                FechaRegistro = TimeZoneInfo.ConvertTimeFromUtc(c.FechaRegistro, argentinaTimeZone)

            }).ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message =  "No se encontró un cliente con ese ID."});

            return Ok(MapToDto(cliente));
        }

        [HttpPost]
        public async Task<ActionResult<ClienteCreateDto>> CreatedCliente([FromBody] ClienteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };
            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email))
                return Conflict("Ya existe un cliente con ese email.");


            _context.Clientes.Add(cliente);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, MapToDto(cliente));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest( new { message = "El ID de la URL no coincide con el del cuerpo."});

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message =  "No se encontró un cliente con ese ID."});


            cliente.Nombre = dto.Nombre;
            cliente.Apellido = dto.Apellido;
            cliente.Email = dto.Email;
            cliente.Telefono = dto.Telefono;
            cliente.Direccion = dto.Direccion;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message =  "No se encontró un cliente con ese ID."});

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private static ClienteDto MapToDto(Cliente cliente) => new()
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Apellido = cliente.Apellido,
            Email = cliente.Email,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion
        };
    }
}
