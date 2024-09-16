using Microsoft.EntityFrameworkCore;
using AllForAllBack.Models;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;

namespace AllForAllBack.Data
{
    public class AllForAllContext : DbContext
    {
        public AllForAllContext(DbContextOptions<AllForAllContext> options)
            : base(options)
        {
        }
        public DbSet<Carrito> Carrito { get; set; }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<MisCompras> MisCompras { get; set; }
        public DbSet<Categoria> Categorias { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MisCompras>()
                .HasKey(m => m.CompraId); // Asegúrate de que la clave primaria esté configurada
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int usuarioId)
        {
            var idParam = new MySqlParameter("@usuarioId", usuarioId);

            var usuarios = await Usuarios
                .FromSqlRaw("CALL SP_GetUsuarioById(@usuarioId)", idParam)
                .ToListAsync();

            return usuarios.FirstOrDefault();
        }

        public async Task<List<Empresa>> GetEmpresasAsync()
        {
            return await Empresas.FromSqlRaw("CALL SP_GetEmpresas()").ToListAsync();
        }

        public async Task<Empresa> GetEmpresaByIdAsync(int empresaId)
        {
            var idParam = new MySqlParameter("@p_EmpresaId", empresaId);

            var empresas = await Empresas
                .FromSqlRaw("CALL SP_GetEmpresaById(@p_EmpresaId)", idParam)
                .ToListAsync();

            return empresas.FirstOrDefault();
        }



        public async Task<int> IniciarSesionAsync(string email, string password)
        {
            var emailParam = new MySqlParameter("@p_email", email);
            var passwordParam = new MySqlParameter("@p_password", HashPassword(password));
            var resultadoParam = new MySqlParameter
            {
                ParameterName = "@p_resultado",
                MySqlDbType = MySqlDbType.Int32,
                Direction = System.Data.ParameterDirection.Output
            };

            try
            {
                await Database.ExecuteSqlRawAsync("CALL sp_IniciarSesion(@p_email, @p_password, @p_resultado)", emailParam, passwordParam, resultadoParam);
                int resultado = (int)resultadoParam.Value;
                Console.WriteLine($"Email: {email}, Password Hash: {HashPassword(password)}, Resultado: {resultado}");
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                throw; // O maneja la excepción según lo necesites
            }
        }






        public async Task CrearCuentaYUsuarioAsync(string email, string password, string rol, string imagen,
                                            string nombre, string apellido, string telefono, string cedula,
                                            string pais, string departamento, string ciudad, DateTime fechaNac,
                                            string direccion)
        {
            try
            {
                string hashedPassword = HashPassword(password);

                var emailParam = new MySqlParameter("@p_email", email);
                var passwordParam = new MySqlParameter("@p_password", hashedPassword);
                var rolParam = new MySqlParameter("@p_rol", rol);
                var imagenParam = new MySqlParameter("@p_imagen", imagen);
                var nombreParam = new MySqlParameter("@p_nombre", nombre);
                var apellidoParam = new MySqlParameter("@p_apellido", apellido);
                var telefonoParam = new MySqlParameter("@p_telefono", telefono);
                var cedulaParam = new MySqlParameter("@p_cedula", cedula);
                var paisParam = new MySqlParameter("@p_pais", pais);
                var departamentoParam = new MySqlParameter("@p_departamento", departamento);
                var ciudadParam = new MySqlParameter("@p_ciudad", ciudad);
                var fechaNacParam = new MySqlParameter("@p_fecha_nac", fechaNac);
                var direccionParam = new MySqlParameter("@p_direccion", direccion);

                await Database.ExecuteSqlRawAsync("CALL SP_CrearCuenta(@p_email, @p_password, @p_rol, @p_imagen, @p_nombre, @p_apellido, @p_telefono, @p_cedula, @p_pais, @p_departamento, @p_ciudad, @p_fecha_nac, @p_direccion)",
                    emailParam, passwordParam, rolParam, imagenParam, nombreParam, apellidoParam, telefonoParam, cedulaParam, paisParam, departamentoParam, ciudadParam, fechaNacParam, direccionParam);
            }
            catch (Exception ex)
            {
                // Registro de la excepción
                Console.WriteLine($"Error al crear cuenta y usuario: {ex.Message}");
                throw; // Re-lanza la excepción para que pueda ser manejada a nivel superior
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("X2")); // "X2" convierte a hexadecimal en mayúsculas
                    if (i % 2 == 1 && i != bytes.Length - 1) // Añadir guión cada 2 caracteres excepto al final
                    {
                        builder.Append("-");
                    }
                }
                return builder.ToString();
            }
        }


        public async Task CrearEmpresaAsync(string nombre, string email, string contraseña)
        {
            try
            {
                string hashedPassword = HashPassword(contraseña);

                var nombreParam = new MySqlParameter("@Nombre", nombre);
                var emailParam = new MySqlParameter("@Email", email);
                var contraseñaParam = new MySqlParameter("@Contraseña", hashedPassword);

                await Database.ExecuteSqlRawAsync("CALL SP_CrearEmpresa(@Nombre, @Email, @Contraseña)",
                    nombreParam, emailParam, contraseñaParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear empresa: {ex.Message}");
                throw;
            }
        }


        public async Task<List<Producto>> GetProductosPorCategoriaAsync(string categoriaNombre)
        {
            var categoriaNombreParam = new MySqlParameter("@categoriaNombre", categoriaNombre);

            return await Productos
                .FromSqlRaw("CALL SP_ProductosPorCategoria(@categoriaNombre)", categoriaNombreParam)
                .ToListAsync();
        }


        public async Task<Producto> GetProductoByIdAsync(int productoId)
        {
            var idParam = new MySqlParameter("@p_ProductoId", productoId);

            var productos = await Productos
                .FromSqlRaw("CALL SP_GetProductoPorId(@p_ProductoId)", idParam)
                .ToListAsync();

            return productos.FirstOrDefault();
        }

        public async Task CrearProductoAsync(string nombre, decimal precio, int categoriaId, int empresaId, int cantidad, string descripcion, string imagen, string codProducto)
        {
            try
            {
                var nombreParam = new MySqlParameter("@p_Nombre", nombre);
                var precioParam = new MySqlParameter("@p_Precio", precio);
                var categoriaIdParam = new MySqlParameter("@p_CategoriaId", categoriaId);
                var empresaIdParam = new MySqlParameter("@p_EmpresaId", empresaId);
                var cantidadParam = new MySqlParameter("@p_Cantidad", cantidad);
                var descripcionParam = new MySqlParameter("@p_Descripcion", descripcion);
                var imagenParam = new MySqlParameter("@p_Imagen", imagen);
                var codProductoParam = new MySqlParameter("@p_CodProducto", codProducto); // Nuevo parámetro


                await Database.ExecuteSqlRawAsync("CALL SP_CrearProducto(@p_Nombre, @p_Precio, @p_CategoriaId, @p_EmpresaId, @p_Cantidad, @p_Descripcion, @p_Imagen, @p_CodProducto)",
                    nombreParam, precioParam, categoriaIdParam, empresaIdParam, cantidadParam, descripcionParam, imagenParam, codProductoParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear producto: {ex.Message}");
                throw;
            }
        }

        public async Task EliminarProductoAsync(int productoId)
        {
            var productoIdParam = new MySqlParameter("@p_ProductoId", productoId);
            await Database.ExecuteSqlRawAsync("CALL SP_EliminarProducto(@p_ProductoId)", productoIdParam);
        }



        public async Task AgregarAlCarritoAsync(int usuarioId, int productoId, int cantidad)
        {
            var usuarioParam = new MySqlParameter("@p_UsuarioId", usuarioId);
            var productoParam = new MySqlParameter("@p_ProductoId", productoId);
            var cantidadParam = new MySqlParameter("@p_Cantidad", cantidad);

            await Database.ExecuteSqlRawAsync("CALL SP_AgregarAlCarrito(@p_UsuarioId, @p_ProductoId, @p_Cantidad)",
                usuarioParam, productoParam, cantidadParam);
        }

        public async Task EliminarDelCarritoAsync(int usuarioId, int productoId)
        {
            var usuarioParam = new MySqlParameter("@p_UsuarioId", usuarioId);
            var productoParam = new MySqlParameter("@p_ProductoId", productoId);

            await Database.ExecuteSqlRawAsync("CALL SP_EliminarDelCarrito(@p_UsuarioId, @p_ProductoId)",
                usuarioParam, productoParam);
        }

        public async Task<List<Carrito>> ObtenerCarritoAsync(int usuarioId)
        {
            var usuarioIdParam = new MySqlParameter("@p_UsuarioId", usuarioId);

            var carrito = await Carrito
                .FromSqlRaw("CALL SP_ObtenerCarrito(@p_UsuarioId)", usuarioIdParam)
                .ToListAsync();

            return carrito;
        }


        public async Task AgregarCompraAsync(int idProd, int userId)
        {
            try
            {
                var idProdParam = new MySqlParameter("@p_IdProd", idProd);
                var userIdParam = new MySqlParameter("@p_UserId", userId);

                await Database.ExecuteSqlRawAsync("CALL SP_AgregarCompra(@p_IdProd, @p_UserId)", idProdParam, userIdParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar compra: {ex.Message}");
                throw;
            }
        }
        public async Task<List<MisCompras>> ObtenerMisComprasAsync(int userId)
        {
            var userIdParam = new MySqlParameter("@p_UserId", userId);

            // Usa `FromSqlRaw` en el DbSet correspondiente
            var misCompras = await MisCompras
                .FromSqlRaw("CALL SP_ObtenerMisCompras(@p_UserId)", userIdParam)
                .ToListAsync();

            return misCompras;
        }

        public async Task<List<Categoria>> GetCategoriasAsync()
        {
            return await Categorias
                .FromSqlRaw("CALL SP_ObtenerCategorias()")
                .ToListAsync();
        }



    }
}
