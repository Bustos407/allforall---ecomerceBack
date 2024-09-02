using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using AllForAllBack.Data;
using AllForAllBack.DTO;

namespace AllForAllBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MisComprasController : ControllerBase
    {
        private readonly AllForAllContext _context;

        public MisComprasController(AllForAllContext context)
        {
            _context = context;
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarCompra([FromBody] AgregarCompraDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Datos inválidos");
            }

            try
            {
                await _context.AgregarCompraAsync(dto.IdProd, dto.UserId);
                return Ok("Compra agregada exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ObtenerMisCompras(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("ID de usuario inválido.");
            }

            try
            {
                var misCompras = await _context.ObtenerMisComprasAsync(userId);
                return Ok(misCompras);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
