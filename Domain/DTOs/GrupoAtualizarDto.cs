namespace Domain.DTOs
{
    public class GrupoAtualizarDto
    {
        public int Id { get; set; }
        public required string Grupo { get; set; }
        public required string NomeUsuarioCriacao { get; set; }
        public required int IdUsuarioCriacao { get; set; }
        public string? JustificativaModificacao { get; set; }
    }
}
