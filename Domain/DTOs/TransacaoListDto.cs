namespace Domain.DTOs
{
    public class TransacaoListDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public int? IdTransacaoPai { get; set; }
    }
}
