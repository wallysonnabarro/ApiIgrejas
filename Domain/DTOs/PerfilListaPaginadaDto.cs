using Domain.Dominio;

namespace Domain.DTOs
{
    public class PerfilListaPaginadaDto
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required ICollection<Transacao> Transacoes { get; set; }
    }
}
