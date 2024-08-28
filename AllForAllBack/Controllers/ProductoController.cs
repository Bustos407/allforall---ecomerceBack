using Microsoft.AspNetCore.Mvc;
using AllForAllBack.Data;
using AllForAllBack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AllForAllBack.DTO;

namespace AllForAllBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AllForAllContext _context;

        public ProductosController(AllForAllContext context)
        {
            _context = context;
        }

        // GET: api/Productos/Categoria/{categoriaId}
        [HttpGet("Categoria/{categoriaId}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductosPorCategoria(int categoriaId)
        {
            try
            {
                var productos = await _context.GetProductosPorCategoriaAsync(categoriaId);
                if (productos == null || !productos.Any())
                {
                    return NotFound($"No se encontraron productos para la categoría con ID {categoriaId}.");
                }

                return Ok(productos);
            }
            catch (Exception ex)
            {
                // Manejo de errores y registro del log
                Console.WriteLine($"Error al obtener productos por categoría: {ex.Message}");
                return StatusCode(500, $"Error al obtener productos por categoría: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            var producto = await _context.GetProductoByIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
    


    [HttpPost("CrearProducto")]
        public async Task<IActionResult> CrearProducto([FromBody] CrearProductoDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Datos de producto inválidos.");
            }

            try
            {
                await _context.CrearProductoAsync(dto.Nombre, dto.Precio, dto.CategoriaId, dto.EmpresaId, dto.Cantidad, dto.Descripcion, dto.Imagen);
                return Ok("Producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear producto: {ex.Message}");
                return StatusCode(500, $"Error al crear producto: {ex.Message}");
            }
        }

        [HttpDelete("EliminarProducto/{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                await _context.EliminarProductoAsync(id);
                return Ok("Producto eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar producto: {ex.Message}");
            }
        }
    }
}
