using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly IAuthorization authorization;
        private readonly IPagamentoRepository pagamentoRepository;

        public PagamentosController(IAuthorization authorization, IPagamentoRepository pagamentoRepository)
        {
            this.authorization = authorization;
            this.pagamentoRepository = pagamentoRepository;
        }

        [HttpPost("confirmar")]
        public async Task<Result<bool>> Confirmar(PagamentoDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await pagamentoRepository.Confirmar(dto, isToken.Result.Email!);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [HttpPost("trasnferir")]
        public async Task<Result<bool>> Transferir(TransferenciaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await pagamentoRepository.Transferidor(dto.IdRecebedor, dto.IdTransferidor, dto.Tipo, isToken.Result.Email!, dto.Siao, dto.Obs);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [HttpPost("cancelar")]
        public async Task<Result<bool>> Cancelar(PagamentoCancelarDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = authorization.DadosToken(token);

            if (isToken.Result.IdentidadeResultado!.Succeeded)
            {
                return await pagamentoRepository.Cancelar(dto, isToken.Result.Email!);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }

        }
    }
}
