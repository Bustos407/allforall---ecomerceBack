﻿namespace AllForAllBack.DTO
{
    public class CrearProductoDto
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
        public int EmpresaId { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }
        public string CodProducto { get; set; }
    }
}
