using AllForAllBack.Data;
using Microsoft.AspNetCore.Mvc;
using AllForAllBack.DTO; // Asegúrate de que esta directiva using esté presente para importar los DTO

[Route("api/[controller]")]
[ApiController]
public class CarritoController : ControllerBase
{
    private readonly AllForAllContext _context;

    public CarritoController(AllForAllContext context)
    {
        _context = context;
    }

    [HttpPost("Agregar")]
    public async Task<IActionResult> AgregarAlCarrito([FromBody] AgregarAlCarritoDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Datos del carrito inválidos.");
        }

        try
        {
            await _context.AgregarAlCarritoAsync(dto.UsuarioId, dto.ProductoId, dto.Cantidad);
            return Ok("Producto agregado al carrito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al agregar producto al carrito: {ex.Message}");
        }
    }

    [HttpPost("Eliminar")]
    public async Task<IActionResult> EliminarDelCarrito([FromBody] EliminarDelCarritoDto dto)
    {
        if (dto == null)
        {
            return BadRequest("Datos del carrito inválidos.");
        }

        try
        {
            await _context.EliminarDelCarritoAsync(dto.UsuarioId, dto.ProductoId);
            return Ok("Producto eliminado del carrito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al eliminar producto del carrito: {ex.Message}");
        }
    }

    [HttpGet("ObtenerCarrito/{usuarioId}")]
    public async Task<ActionResult<IEnumerable<CarritoDto>>> ObtenerCarrito(int usuarioId)
    {
        try
        {
            var carrito = await _context.ObtenerCarritoAsync(usuarioId);

            if (carrito == null || !carrito.Any())
            {
                return NotFound("Carrito vacío o usuario no encontrado.");
            }

            return Ok(carrito);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener el carrito: {ex.Message}");
            return StatusCode(500, $"Error al obtener el carrito: {ex.Message}");
        }
    }

}
