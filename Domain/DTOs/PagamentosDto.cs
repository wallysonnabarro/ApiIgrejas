namespace Domain.DTOs
{
    public class PagamentosDto
    {
        public decimal Dinheiro { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public decimal CreditoParcelado { get; set; }
        public decimal Pix { get; set; }
        public decimal Receber { get; set; }
        public decimal Descontar { get; set; }
        public decimal Total { get; set; }
        public decimal Tipo { get; set; }
    }
}
