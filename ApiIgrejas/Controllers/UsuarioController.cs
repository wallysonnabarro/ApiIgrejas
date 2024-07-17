using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace ApiIgrejas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticarManager;
        private readonly IUserService _userService;
        private readonly IAntiforgery _antiforgery;
        private readonly IAuthorization _authorization;
        private readonly IUsuarioRepository _userManager;
        private readonly IUserService _userServices;
        private readonly IRoleRepository _roleRepository;
        private readonly ContextDb _db;

        public UsuarioController(ContextDb db, IRoleRepository roleRepository, IUsuarioRepository userManager, IUserService userService, IAntiforgery antiforgery, IAuthenticationRepository authenticarManager, IAuthorization authorization, IUserService userServices)
        {
            _db = db;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _userService = userService;
            _antiforgery = antiforgery;
            _authenticarManager = authenticarManager;
            _authorization = authorization;
            _userServices = userServices;
        }

        /// <summary>
        /// Método de atorização.
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(200, "Login", typeof(Token))]
        [ProducesResponseType(typeof(Token), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Login(LoginDTO loginDTO)
        {
            if (loginDTO != null)
            {
                var isValid = await _userService.ValidarLogin(loginDTO.Email, loginDTO.Senha);

                if (isValid.Succeeded)
                {
                    var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

                    Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions
                    {
                        HttpOnly = false
                    });

                    var token = await _authenticarManager.AuthenticateUser(loginDTO.Email, loginDTO.Senha);

                    return Results.Accepted(null, token);
                }
                else
                {
                    return Results.Empty;
                }
            }
            else
            {
                return Results.Empty;
            }
        }

        /// <summary>
        /// Método público que válida o token 
        /// </summary>
        /// <param name="toke"></param>
        /// <returns></returns>
        [HttpPost("auth-token")]
        [SwaggerResponse(200, "Autenticar", typeof(TokenGerenciar))]
        [ProducesResponseType(typeof(TokenGerenciar), 200)]
        public async Task<IResult> AuthToken(TokenDTO toke)
        {
            if (!toke.Equals(""))
            {
                TokenGerenciar isValid = await _authorization.IsAuthTokenValid(toke.Token);
                return Results.Json(isValid);
            }
            else
            {
                var retorno = new TokenGerenciar().IdentidadeResultado = Identidade.Failed(new IdentidadeError { Code = "erro", Description = "Token vazio." });
                return Results.Json(retorno);
            }
        }

        [HttpPost("lista-paginada")]
        [SwaggerResponse(200, "Paginação de usuários", typeof(Result<Paginacao<UsuarioDto>>))]
        [ProducesResponseType(typeof(Result<Paginacao<UsuarioDto>>), 200)]
        public async Task<IActionResult> Paginar(PageWrapper dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this._authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = _authorization.DadosToken(token);

            var result = await _userManager.Paginacao(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpPost("lista-paginada-novo")]
        [SwaggerResponse(200, "Paginação de usuários", typeof(Result<Paginacao<UsuarioDto>>))]
        [ProducesResponseType(typeof(Result<Paginacao<UsuarioDto>>), 200)]
        public async Task<IActionResult> PaginarNovo(PageWrapper dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this._authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = _authorization.DadosToken(token);

            var result = await _userManager.PaginacaoLista(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

        [HttpGet("detalhar/{id}")]
        [SwaggerResponse(200, "deatlhar", typeof(Result<UsuarioDetalharDto>))]
        [ProducesResponseType(typeof(Result<UsuarioDetalharDto>), 200)]
        public async Task<IActionResult> Detalhar(int id)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this._authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = _authorization.DadosToken(token);

            var result = await _userManager.UserDetalhe(id);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }


        [HttpPost("novo")]
        [SwaggerResponse(200, "novo usuário", typeof(Result<Paginacao<UsuarioDto>>))]
        [ProducesResponseType(typeof(Result<Paginacao<UsuarioDto>>), 200)]
        public async Task<IActionResult> Novo(NovoUsuarioDto dto)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this._authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var isToken = _authorization.DadosToken(token);

            var result = await _userManager.Novo(dto, isToken.Result.Email!);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }


        [HttpGet("validar-token")]
        [SwaggerResponse(200, "validar token", typeof(bool))]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ValidarToken()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token == null) return Unauthorized(new { mensagem = "Acesso não autorizado" });

            var resultToken = await this._authorization.IsAuthTokenValid(token);

            if (!resultToken.IdentidadeResultado!.Succeeded) return Unauthorized(new { mensagem = "Acesso não autorizado" });
            else return Ok(true);
        }


        [HttpPost("redefinir-senha")]
        [SwaggerResponse(200, "redefinir senha", typeof(Result<bool>))]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> RedefinirSenha(LoginDTO loginDTO)
        {
            var result = await _userManager.RedefinirSenha(loginDTO);

            if (result.Succeeded)
                return Ok(result);
            else
                return BadRequest(new { mensagem = result.Errors.Min(x => x.mensagem) });
        }

    }
}
