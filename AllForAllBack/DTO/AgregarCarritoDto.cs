public class AgregarAlCarritoDto
{
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public class EliminarDelCarritoDto
{
    public int UsuarioId { get; set; }
    public int ProductoId { get; set; }
}

public class CarritoDto
{
    public int CarritoId { get; set; }
    public string ProductoNombre { get; set; }
    public int Cantidad { get; set; }
    public decimal Precio { get; set; }
}
