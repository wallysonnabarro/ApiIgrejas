namespace Domain.DTOs
{
    public class ListPagamento
    {
        public string Siao { get; set; }
        public int qtd { get; set; }
        public string Tribo { get; set; }
        public string Nome { get; set; }
        public string obs { get; set; }
        public string Sexo { get; set; }
        public decimal dinheiro { get; set; }
        public decimal debito { get; set; }
        public decimal credVista { get; set; }
        public decimal credParcelado { get; set; }
        public decimal tedPix { get; set; }
        public decimal descontar { get; set; }
        public decimal receber { get; set; }
    }
}
