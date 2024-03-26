namespace Domain.Dominio
{
    public class TokenGerenciar
    {
        public required Identidade IdentidadeResultado { get; set; }
        public required string Email { get; set; }
        public required string Nome { get; set; }
        public string? Role { get; set; }
    }
}
