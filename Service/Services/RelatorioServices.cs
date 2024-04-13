using AspNetCore.Reporting;
using AspNetCore.Reporting.ReportExecutionService;
using Domain.Dominio;
using Domain.DTOs;
using Service.Interface;
using System.Text;

namespace Service.Services
{
    public class RelatorioServices : IRelatorioServices
    {
        private readonly IRenderType _renderType;

        public RelatorioServices(IRenderType renderType)
        {
            _renderType = renderType;
        }

        public async Task<Result<byte[]>> gerarRelatorioByte(Result<List<CheckInReports>> lista)
        {
            try
            {
                string diretorioBase = AppDomain.CurrentDomain.BaseDirectory;

                var caminhoArquivo = Path.Combine("Reports", "rdCheckin.rdlc");

                string caminhoCompleto = Path.Combine(diretorioBase, caminhoArquivo);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1252");
                LocalReport report = new LocalReport(caminhoCompleto);
                
                report.AddDataSource("dsCheckin", lista.Dados);

                var result = await Task.Run(() =>
                {
                    return report.Execute(_renderType.GetRenderType("excel"), 1);
                });

                return Result<byte[]>.Sucesso(result.MainStream);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }
    }
}
