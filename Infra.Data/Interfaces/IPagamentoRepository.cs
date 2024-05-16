using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IPagamentoRepository
    {
        Task<Result<bool>> Confirmar(PagamentoDto dto, string EmailUser);
        Task<Result<bool>> Cancelar(PagamentoCancelarDto dto, string EmailUser);
        Task<Result<bool>> Atualizar(PagamentoAtualizarDto dto);
        Task<Result<PagamentoAtualizarDto>> BuscarAtualizar(PagamentoCancelarDto dto);
        Task<Result<List<PagamentosDto>>> GetPagamento(int id);
        Task<Result<bool>> Transferidor(int idRecebedor, int idTransferidor, int tipo, string EmailUser, int siao, string obs);


        Task<Result<string>> RegistrarListaSaida(List<ItemPagamentoSaidaDto> dto, string EmailUser, int id);
        Task<Result<List<ListPagamento>>> ListaPagamentoVoluntariosExcel(int id);
        Task<Result<List<ListPagamento>>> ListaPagamentoConcetadosExcel(int id);

    }
}
