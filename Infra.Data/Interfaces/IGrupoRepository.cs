using Domain.Dominio;
using Domain.Dominio.menus;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IGrupoRepository
    {
        Task<Result<bool>> Novo(GrupoDto dto);
        Task<Result<Paginacao<GrupoPaginado>>> Paginacao(PageWrapper wrapper);
        Task<Result<Grupos>> BuscarPorId(int id);
        Task<Result<bool>> Atualizar(GrupoAtualizarDto dto);
    }
}
