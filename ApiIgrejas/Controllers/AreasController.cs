using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreasRepository _areasRepository;
        private readonly IAuthorization authorization;

        public AreasController(IAreasRepository areasRepository, IAuthorization authorization)
        {
            _areasRepository = areasRepository;
            this.authorization = authorization;
        }

        [Authorize]
        [HttpPost("novo")]
        public async Task<Result<bool>> Novo(AreaDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _areasRepository.Novo(dto, isToken.Email!);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("listar")]
        public async Task<Result<Paginacao<Area>>> Paginado(PageWrapper wrapper)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _areasRepository.Paginacao(wrapper, isToken.Email!);
            }
            else
            {
                return Result<Paginacao<Area>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("editar")]
        public async Task<Result<bool>> Editar(Area area)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _areasRepository.Editar(area, isToken.Email);
            }
            else
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [Authorize]
        [HttpPost("detalhar/{id}")]
        public async Task<Result<Area>> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isToken = await authorization.DadosToken(token);

            if (isToken.IdentidadeResultado!.Succeeded)
            {
                return await _areasRepository.Detalhar(id, isToken.Email);
            }
            else
            {
                return Result<Area>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
            }
        }

        [HttpGet("getAreas")]
        public async Task<Result<List<Area>>> getAreas()
        {
            return await _areasRepository.GetAll();
        }
    }
}
