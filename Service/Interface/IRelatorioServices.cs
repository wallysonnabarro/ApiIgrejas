using Domain.Dominio;
using Domain.DTOs;

namespace Service.Interface
{
    public interface IRelatorioServices
    {
        Task<Result<byte[]>> gerarRelatorioByte(Result<List<CheckInReports>> lista);
    }
}
