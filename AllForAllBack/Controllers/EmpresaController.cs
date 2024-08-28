using Microsoft.AspNetCore.Mvc;
using AllForAllBack.Data;
using AllForAllBack.Models; // Agrega esta línea
using AllForAllBack.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllForAllBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly AllForAllContext _context;

        public EmpresasController(AllForAllContext context)
        {
            _context = context;
        }

        // GET: api/Empresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            var empresas = await _context.GetEmpresasAsync();
            return Ok(empresas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresaById(int id)
        {
            try
            {
                var empresa = await _context.GetEmpresaByIdAsync(id);
                if (empresa == null)
                {
                    return NotFound("Empresa no encontrada.");
                }

                return Ok(empresa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener empresa: {ex.Message}");
            }
        }



        [HttpPost("CrearEmpresa")]
        public async Task<IActionResult> CrearEmpresa([FromBody] CrearEmpresaDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Datos de empresa inválidos.");
            }

            try
            {
                // Llamar al método del contexto para crear la empresa y la cuenta
                await _context.CrearEmpresaAsync(dto.Nombre, dto.Email, dto.Contraseña);

                return Ok("Empresa y cuenta creadas exitosamente.");
            }
            catch (Exception ex)
            {
                // Manejo de errores y log
                Console.WriteLine($"Error al crear empresa: {ex.Message}");
                return StatusCode(500, $"Error al crear empresa: {ex.Message}");
            }
        }

    }
}
