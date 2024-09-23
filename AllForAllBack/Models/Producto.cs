namespace AllForAllBack.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public int CategoriaId { get; set; }
        public int EmpresaId { get; set; }
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public string? CodProducto { get; set; }  
        public int ProdVendidos { get; set; }
        public string? CategoriaNombre { get; set; }
        public string? EmpresaNombre { get; set; }
    }
}

