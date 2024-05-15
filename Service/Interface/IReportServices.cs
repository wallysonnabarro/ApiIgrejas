using Domain.Dominio;
using Domain.DTOs;

namespace Service.Interface
{
    public interface IReportServices
    {
        Task<Result<byte[]>> arquivoExcelByte(List<ListPagamento> lista, string rdlcPath, string ds);
    }
}
