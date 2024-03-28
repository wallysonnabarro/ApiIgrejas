namespace Domain.DTOs
{
    public class GrupoPaginado
    {
        public int Id { get; set; }
        public required string Grupo { get; set; }
        public required string NomeUsuarioCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string? JustificativaModificacao { get; set; }
    }
}
