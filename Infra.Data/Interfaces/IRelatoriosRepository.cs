using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IRelatoriosRepository
    {
        Task<Result<DadosRelatorio<List<CheckInReports>>>> GetByIdHomens(ParametrosEvento dto);
        Task<Result<FichasDto<List<CheckInReports>>>> GetByConectados(ParametrosConectados dto);
        Task<Result<DadosReaderRelatorio>> NovoConectadoReader(DadosReaderRelatorioDto dto);
    }
}
