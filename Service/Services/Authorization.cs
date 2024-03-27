using Domain.Dominio;
using Microsoft.AspNetCore.Http;
using Service.Interface;

namespace Service.Services
{
    public class Authorization : IAuthorization
    {
        private readonly ITokenService _tokenService;

        public Authorization(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenGerenciar> IsAuthTokenValid(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var isValid = await _tokenService.TryValidateToken(token);
                if (isValid.IdentidadeResultado!.Succeeded)
                {
                    return isValid;
                }
            }

            return new TokenGerenciar { Email = "", Nome = "", IdentidadeResultado = Identidade.Failed(new IdentidadeError { Code = "15", Description = "Token invalid" }) };
        }

        public async Task<TokenGerenciar> DadosToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var isValid = await _tokenService.TryValidateToken(token);
                if (isValid.IdentidadeResultado.Succeeded)
                {
                    return isValid;
                }
            }

            return new TokenGerenciar { Email = "", Nome = "", IdentidadeResultado = Identidade.Failed(new IdentidadeError { Code = "15", Description = "Token invalid" }) };
        }
    }
}
