
using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IConfiguracoes
    {
        Task<Result<string>> Novo(string dto, string email);
        Task<Result<Paginacao<TiposSaidaDto>>> Paginacao(PageWrapper wrapper, string email);
        Task<Result<string>> Editar(TiposSaidaDto area, string email);
        Task<Result<TiposSaidaDto>> Detalhar(int id, string email);
    }
}
