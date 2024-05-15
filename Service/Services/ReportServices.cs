using AspNetCore.Reporting;
using Domain.Dominio;
using Domain.DTOs;
using Service.Interface;
using System.Text;

namespace Service.Services
{
    public class ReportServices : IReportServices
    {
        public async Task<Result<byte[]>> arquivoExcelByte(List<ListPagamento> lista, string rdlcPath, string ds)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");
                LocalReport report = new LocalReport(rdlcPath);

                report.AddDataSource(ds, lista);

                var result = await Task.Run(() =>
                {
                    try
                    {
                        return report.Execute(GetRenderType("pdf"), 1);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Erro ao criar o reporte. (Run) " + e.Message + ", rdlcPath: " + rdlcPath);
                    }
                });

                return Result<byte[]>.Sucesso(result.MainStream);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao criar o reporte. (GenerateReport) " + e.Message + ", lista: " + lista + ", rdlcPath: " + rdlcPath);
            }
        }

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;
            switch (reportType.ToLower())
            {
                default:
                case "pdf":
                    renderType = RenderType.Pdf;
                    break;
                case "word":
                    renderType = RenderType.Word;
                    break;
                case "excel":
                    renderType = RenderType.Excel;
                    break;
            }

            return renderType;
        }

    }
}
