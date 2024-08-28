namespace AllForAllBack.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? Administrador_id { get; set; }
        public int ProdVendidos { get; set; }
        public int? CuentaID { get; set; }

    }
}
