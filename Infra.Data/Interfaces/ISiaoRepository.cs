using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface ISiaoRepository
    {
        Task<Result<string>> Novo(SiaoNovoDto dto, string email);
        Task<Result<Paginacao<Evento>>> Paginacao(PageWrapper wrapper, string email);
        Task<Result<bool>> Editar(Evento siao, string email);
        Task<Result<bool>> EditarStatus(int id, int status, string email);
        Task<Result<Evento>> Detalhar(int id, string email);
        Task<Result<List<EventosAtivosDto>>> GetEmIniciado();
        Task<Result<EventosAtivosDto>> ConfirmarToken(ConfirmarTokenDto dto);
    }
}
