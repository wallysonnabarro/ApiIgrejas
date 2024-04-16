namespace Domain.Dominio
{
    public class DadosReaderRelatorio
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public required string Titulo { get; set; }
        public required string Subtitulo { get; set; }
        public byte[]? Imagem { get; set; }
    }
}
