namespace Domain.Dominio
{
    public class PagamentoSaida
    {
        public int Id { get; set; }
        public required string Descricao { get; set; }
        public required string FormaPagamento { get; set; }
        public required decimal Valor { get; set; }
        public required int Tipo { get; set; }
        public string? TipoNome { get; set; }
    }
}
