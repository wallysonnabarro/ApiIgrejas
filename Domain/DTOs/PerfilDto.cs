using Domain.Dominio;

namespace Domain.DTOs
{
    public class PerfilDto
    {
        public required string Nome { get; set; }
        public required List<int> Transacoes { get; set; }
    }
}
