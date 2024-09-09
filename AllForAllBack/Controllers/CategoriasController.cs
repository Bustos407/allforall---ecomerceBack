using Microsoft.AspNetCore.Mvc;
using AllForAllBack.Data;
using AllForAllBack.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace AllForAllBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AllForAllContext _context;

        public CategoriasController(AllForAllContext context)
        {
            _context = context;
        }

   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            try
            {
                var categorias = await _context.GetCategoriasAsync();
                if (categorias == null || !categorias.Any())
                {
                    return NotFound("No se encontraron categorías.");
                }

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                // Manejo de errores y registro del log
                Console.WriteLine($"Error al obtener categorías: {ex.Message}");
                return StatusCode(500, $"Error al obtener categorías: {ex.Message}");
            }
        }
    }
}
