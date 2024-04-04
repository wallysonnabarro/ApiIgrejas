namespace Domain.DTOs
{
    public class FichaPagamento
    {
        public int Count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public required DadosCard Feminino { get; set; }
        public required DadosCard Masculino { get; set; }
        public required List<ListaInscricoes> Dados { get; set; }
    }
}
