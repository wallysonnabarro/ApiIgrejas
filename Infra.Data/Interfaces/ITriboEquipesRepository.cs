using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface ITriboEquipesRepository
    {
        Task<Result<Paginacao<TriboEquipe>>> Paginacao(PageWrapper wrapper, string email);
        Task<Result<TriboEquipe>> Novo(TriboNovoDto dto, string email);
        Task<Result<TriboEquipe>> Editar(TriboEquipe dto);
        Task<Result<List<TriboSelectede>>> ListaSelected(string email);
        Task<Result<TriboEquipe>> Detalhar(int id, string email);
    }
}
