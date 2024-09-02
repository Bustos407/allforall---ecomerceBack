namespace AllForAllBack.Models
{
    public class MisCompras
    {
        public int CompraId { get; set; }  // Asegúrate de que este campo coincide con el alias en la consulta
        public string Prod { get; set; }
        public int ID_Prod { get; set; }
        public int UserId { get; set; }
    }

}
