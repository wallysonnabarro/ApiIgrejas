﻿using Domain.Dominio;
using Domain.DTOs;

namespace Infra.Data.Interfaces
{
    public interface IFichaRepository
    {
        Task<Result<bool>> NovoConectado(FichaConectadoDto dto);
        Task<Result<bool>> NovoLider(FichaLiderDto dto);
        Task<Result<FichaPagamento>> GetFichasInscricoes(FichaParametros parametros);
        Task<Result<FichaPagamento>> listaSearch(FichaParametros parametros);
        Task<Result<FichaPagamento>> GetFichasInscricoesNaoconfirmados(FichaParametros parametros);
    }
}
