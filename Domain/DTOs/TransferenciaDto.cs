namespace Domain.DTOs
{
    public class TransferenciaDto
    {
        public int IdRecebedor { get; set; }
        public int IdTransferidor { get; set; }
        public int Tipo { get; set; }
        public int Siao { get; set; }
        public string Obs { get; set; }
    }
}
