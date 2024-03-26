using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IContratoRepository
    {
        Task<Result<ContratoDto>> NovoContrato(ContratoDto dto);
    }
}
