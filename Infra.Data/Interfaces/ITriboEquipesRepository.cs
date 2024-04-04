using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface ITriboEquipesRepository
    {
        Task<Result<Paginacao<TriboEquipe>>> Paginacao(PageWrapper wrapper);
        Task<Result<TriboEquipe>> Novo(TriboNovoDto dto);
        Task<Result<TriboEquipe>> Editar(TriboEquipe dto);
        Task<Result<List<TriboSelectede>>> ListaSelected();
        Task<Result<TriboEquipe>> Detalhar(int id);
    }
}
