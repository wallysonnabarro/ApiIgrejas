using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface ISiaoRepository
    {
        Task<Result<bool>> Novo(SiaoNovoDto dto);
        Task<Result<Paginacao<Siao>>> Paginacao(PageWrapper wrapper);
        Task<Result<bool>> Editar(Siao siao);
        Task<Result<bool>> EditarStatus(int id, int status);
        Task<Result<Siao>> Detalhar(int id);
    }
}
