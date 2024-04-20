using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IAreasRepository
    {
        Task<Result<bool>> Novo(AreaDto dto, string email);
        Task<Result<Paginacao<Area>>> Paginacao(PageWrapper wrapper, string email);
        Task<Result<bool>> Editar(Area area, string email);
        Task<Result<Area>> Detalhar(int id, string email);
        Task<Result<List<Area>>> GetAll(string token);
    }
}
