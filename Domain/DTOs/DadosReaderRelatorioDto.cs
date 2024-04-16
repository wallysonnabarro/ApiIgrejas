namespace Domain.DTOs
{
    public class DadosReaderRelatorioDto
    {
        public int TipoRelatorio { get; set; }
        public required string Titulo { get; set; }
        public required string Subtitulo { get; set; }
        public byte[]? Imagem { get; set; }
    }
}
