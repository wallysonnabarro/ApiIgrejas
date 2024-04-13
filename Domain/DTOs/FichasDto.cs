namespace Domain.DTOs
{
    public class FichasDto<T>
    {
        public required string TituloRelatorio { get; set; }
        public required string SubTituloRelatorio { get; set; }

        public required T Dados { get; set; }
    }
}
