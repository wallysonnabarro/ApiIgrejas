using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface ITransacaoRepository
    {
        Task<Result<Transacao>> novo(TransacaoDto dto);
        Task<Result<Paginacao<TransacaoListDto>>> Paginacao(PageWrapper wrapper);
    }
}
