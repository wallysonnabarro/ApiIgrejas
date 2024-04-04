namespace Domain.DTOs
{
    public class PagamentoDto
    {
        public decimal? Dinheiro { get; set; }
        public decimal? Debito { get; set; }
        public decimal? Credito { get; set; }
        public decimal? CreditoParcelado { get; set; }
        public int Parcelas { get; set; }
        public decimal? Pix { get; set; }
        public int Desistente { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal? Receber { get; set; }
        public decimal? Descontar { get; set; }
        public string? Observacao { get; set; }
        public int FichaConsumidor { get; set; }
        public int Voluntario { get; set; }
        public int Siao { get; set; }
        public int Usuario { get; set; }
    }
}
