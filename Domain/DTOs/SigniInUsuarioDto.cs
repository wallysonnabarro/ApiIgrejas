using Domain.Dominio;

namespace Domain.DTOs
{
    public class SigniInUsuarioDto
    {
        public Usuario? User { get; set; }

        public PerfilListaPaginadaDto? Role { get; set; }
        public required SignInResultado SignInResultado { get; set; }
    }
}
