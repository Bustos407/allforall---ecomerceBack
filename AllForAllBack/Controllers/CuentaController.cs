using Microsoft.AspNetCore.Mvc;
using AllForAllBack.Data;
using AllForAllBack.Models;
using System.Threading.Tasks;

namespace AllForAllBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AllForAllContext _context;

        public AuthController(AllForAllContext context)
        {
            _context = context;
        }


        [HttpPost("IniciarSesion")]
        public async Task<ActionResult<int>> IniciarSesion([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var resultado = await _context.IniciarSesionAsync(loginRequest.Email, loginRequest.Password);
                if (resultado == 0)
                {
                    return Unauthorized("La cuenta no existe"); // 0 indica inicio de sesión fallido
                }
                else
                {
                    return Ok(resultado); // 1, 2 o 3 indica el rol del usuario
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores y registro del log
                Console.WriteLine($"Error al iniciar sesión: {ex.Message}");
                return StatusCode(500, $"Error al iniciar sesión: {ex.Message}");
            }
        }




        [HttpPost("CrearCuenta")]
        public async Task<ActionResult> CrearCuenta([FromBody] CrearCuentaRequest crearCuentaRequest)
        {
            try
            {
                await _context.CrearCuentaYUsuarioAsync(crearCuentaRequest.Email, crearCuentaRequest.Password, crearCuentaRequest.Rol, crearCuentaRequest.Imagen,
                                                        crearCuentaRequest.Nombre, crearCuentaRequest.Apellido, crearCuentaRequest.Telefono, crearCuentaRequest.Cedula,
                                                        crearCuentaRequest.Pais, crearCuentaRequest.Departamento, crearCuentaRequest.Ciudad, crearCuentaRequest.FechaNacimiento,
                                                        crearCuentaRequest.Direccion);

                return Ok("Cuenta creada exitosamente");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CrearCuentaRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Cedula { get; set; }
        public string Pais { get; set; }
        public string Departamento { get; set; }
        public string Ciudad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Direccion { get; set; }
    }
}
