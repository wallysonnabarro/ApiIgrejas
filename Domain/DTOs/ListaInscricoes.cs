namespace Domain.DTOs
{
    public class ListaInscricoes
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int Sexo { get; set; }
        public int Idade { get; set; }
        public int Confirmacao { get; set; }
        public decimal? Pago { get; set; }
        public decimal? Receber { get; set; }
    }
}
