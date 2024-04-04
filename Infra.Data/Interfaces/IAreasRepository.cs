using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IAreasRepository
    {
        Task<Result<bool>> Novo(AreaDto dto);
        Task<Result<Paginacao<Area>>> Paginacao(PageWrapper wrapper);
        Task<Result<bool>> Editar(Area area);
        Task<Result<Area>> Detalhar(int id);
    }
}
