using Domain.Dominio;

namespace Service.Interface
{
    public interface IAuthorization
    {
        public Task<TokenGerenciar> IsAuthTokenValid(string? token);
        public Task<TokenGerenciar> DadosToken(string token);
    }
}
