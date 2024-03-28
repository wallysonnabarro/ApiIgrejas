namespace Domain.DTOs
{
    public class TransacaoDto
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public int? IdTransacaoPai { get; set; }
        public int Ordenacao { get; set; }
        public required string Rota { get; set; }
        public int Status { get; set; }
        public DateTime DataCadastro { get; set; }
        public int StMenu { get; set; }
        public int StFormulario { get; set; }
        public int StFuncao { get; set; }
        public int StControle { get; set; }
    }
}
