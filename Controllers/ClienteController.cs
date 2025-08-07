using ClienteApi.Data;
using ClienteApi.Dtos;
using ClienteApi.Mappers;
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
    
        // Inyección del DbContext
        public ClienteController(ClienteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.OrderBy(i => i.Id)
            .Select(r => ClienteMapper.MapToDto(r))
            .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "No se encontró un cliente con ese ID." });

            return Ok(ClienteMapper.MapToDto(cliente));
        }

        [HttpPost]
        public async Task<ActionResult<ClienteCreateDto>> CreateCliente([FromBody] ClienteCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            dto.Nombre = dto.Nombre.Clean();
            dto.Apellido = dto.Apellido.Clean();
            dto.Email = dto.Email.Clean().ToLower();  // Normalizar email
            dto.Telefono = dto.Telefono.Clean();
            dto.Direccion = dto.Direccion.Clean();

            var cliente = ClienteMapper.MapToModel(dto);
            if (await _context.Clientes.AnyAsync(d => d.Email == dto.Email))
                return Conflict(new { message = "Ya existe un cliente con ese Email" });

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, ClienteMapper.MapToDto(cliente));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return Conflict(new { message = "El ID de la URL no coincide con el del cuerpo." });
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "No se encontró un cliente con ese ID." });

            var originEmail = cliente.Email;

            ClienteMapper.MapToModel(dto, cliente);

            var newEmail = dto.Email.Clean().ToLower();
            // Verificar si el nuevo email ya está registrado por otro cliente
            if (originEmail != newEmail)
            {
                var emailRegistrado = await _context.Clientes.AnyAsync(e => e.Email.ToLower() == newEmail && e.Id != id);
                if (emailRegistrado)
                    return Conflict(new { message = "Ya existe un registro con ese email" });

                cliente.Email = newEmail;
            }

            await _context.SaveChangesAsync();

            return Ok(ClienteMapper.MapToDto(cliente));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound(new { message = "No se encontró un cliente con ese ID." });

            _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
