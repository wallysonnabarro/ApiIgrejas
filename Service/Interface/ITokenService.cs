using Domain.Dominio;

namespace Service.Interface
{
    public interface ITokenService
    {
        public Task<Token> GenerateToken(Usuario user, Role role);
        public Task<TokenGerenciar> TryValidateToken(string token);
    }
}
