using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
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
        private readonly IRoleRepository _roleRepository;
        private readonly ContextDb _db;

        public UsuarioController(ContextDb db, IRoleRepository roleRepository, IUsuarioRepository userManager, IUserService userService, IAntiforgery antiforgery, IAuthenticationRepository authenticarManager, IAuthorization authorization)
        {
            _db = db;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _userService = userService;
            _antiforgery = antiforgery;
            _authenticarManager = authenticarManager;
            _authorization = authorization;
        }

        /// <summary>
        /// Método de atorização.
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Identidade>(StatusCodes.Status200OK)]
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

        [HttpPost("paginacao")]
        //[Authorize]
        public async Task<IActionResult> Paginar(PageWrapper dto)
        {
            if (dto != null)
            {
                return Accepted(await _userManager.Paginacao(dto));
            }
            else
            {
                return BadRequest("Dados inválidos.");
            }
        }

        /// <summary>
        /// Este endpoint, deverá ser removido na publicação.
        /// </summary>
        //[HttpGet("cadastro-develope")]
        //public async Task<Identidade> CadastroDeveloper()
        //{
        //    if (await _db.Users.AnyAsync(x => x.Cpf.Equals("009.873.571-31")))
        //    {
        //        return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });
        //    }

        //    var contrato = _db.Contratos.FirstOrDefault(x => x.Id == 1);

        //    if (contrato == null) return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });

        //    if (!await _roleRepository.IsValid("DESENVOLVEDOR")) await _roleRepository.Insert(0);

        //    var role = await _roleRepository.Get("DESENVOLVEDOR");

        //    var tribo = _db.TribosEquipes.FirstOrDefaultAsync(x => x.Nome.Equals("MAANAIM"));

        //    if (tribo == null) return Identidade.Failed(new IdentidadeError { Code = "", Description = "" });

        //    var triboEquipe = new TriboEquipe { Nome = "MAANAIM", Status = 1 };

        //    _db.Add(triboEquipe);
        //    _db.SaveChanges();

        //    var user = new UsuarioDto()
        //    {
        //        Cpf = "009.873.571-31",
        //        Email = "wallyson.a3@gmail.com",
        //        NormalizedEmail = "WALLYSON.A3@GMAIL.COM",
        //        Nome = "Wallyson Lopes",
        //        NormalizedUserName = "WALLYSON LOPES",
        //        Contrato = contrato,
        //        Password = "389419wE1e@",
        //        Role = role,
        //        TriboEquipe = triboEquipe,
        //        UserName = "wallyson.a3@gmail.com",
        //        TwoFactorEnabled = false,
        //    };

        //    return await _userManager.AddUserWithSecurePassword(user);
        //}
    }
}
