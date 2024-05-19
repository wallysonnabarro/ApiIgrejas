using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IContratoRepository
    {
        Task<Result<ContratoDto>> NovoContrato(ContratoDto dto);
        Task<Result<Paginacao<Contrato>>> Paginacao(PageWrapper wrapper);
        Task<Result<Contrato>> Update(ContratoDto dto, int id);
        Task<Result<Contrato>> GetResult(string email);
        Task<Result<Contrato>> GetResult(int id);
        Task<Result<List<ContratoSelectedDto>>> GetList();

    }
}
