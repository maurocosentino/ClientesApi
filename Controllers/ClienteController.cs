using ClienteApi.Data;
using ClienteApi.Dtos;
using ClienteApi.Models;
using ClienteApi.Utils;
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
        public async Task<ActionResult<ClienteCreateDto>> CreateCliente([FromBody] ClienteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Normalizar y limpiar todos los campos ANTES de cualquier lógica
            dto.Nombre = dto.Nombre.Clean();
            dto.Apellido = dto.Apellido.Clean();
            dto.Email = dto.Email.Clean().ToLower();  // Normalizar email
            dto.Telefono = dto.Telefono.Clean();
            dto.Direccion = dto.Direccion.Clean();

            // Verificar unicidad del email (siempre necesario en creación)
            if (await _context.Clientes.AnyAsync(c => c.Email == dto.Email))
                return Conflict(new { message = "Ya existe un cliente con ese email." });

            // Crear entidad con datos ya normalizados
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Email = dto.Email,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, MapToDto(cliente));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (id != dto.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "No se encontró un cliente con ese ID." });
            
            // Guardar valores originales para comparación
            var originalEmail = cliente.Email;
            
            // Actualizar propiedades
            cliente.Nombre = dto.Nombre.Clean();
            cliente.Apellido = dto.Apellido.Clean();
            cliente.Telefono = dto.Telefono.Clean();
            cliente.Direccion = dto.Direccion.Clean();
            
            // Manejo especial para email
            var newEmail = dto.Email.Clean().ToLower();
            if (originalEmail != newEmail)
            {
                if (await _context.Clientes.AnyAsync(c => c.Email == newEmail))
                    return Conflict(new { message = "Ya existe un cliente con ese email." });
                
                cliente.Email = newEmail;
            }
            
                await _context.SaveChangesAsync();
                
                return Ok(MapToDto(cliente));
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
