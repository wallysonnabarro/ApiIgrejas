namespace Domain.DTOs
{
    public class DadosRelatorio<T>
    {
        public required byte[] Imagem { get; set; }
        public required string TituloRelatorio { get; set; }
        public required string SubTituloRelatorio { get; set; }

        public required T Dados { get; set; }
    }
}
