using AutoMapper;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace Infra.Data.Respository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ContextDb _context;
        private readonly IUserService _userServices;
        private readonly Mapper _mapper;

        public UsuarioRepository(ContextDb context, IUserService userServices)
        {
            _context = context;
            _userServices = userServices;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
        }


        public void AddRoleToUser(decimal userId, Role role)
        {
            throw new NotImplementedException();
        }

        public async Task<Identidade> AddUserWithSecurePassword(UsuarioDto user)
        {
            try
            {
                byte[] salt = await _userServices.GenerateSalt();

                var passwordHashSalt = Convert.ToBase64String(await _userServices.GeneratePasswordHash(user.Password, salt));
                var usuario = new Usuario
                {
                    Contrato = user.Contrato,
                    Cpf = user.Cpf,
                    Email = user.Email,
                    Nome = user.Nome,
                    NormalizedEmail = user.NormalizedEmail,
                    NormalizedUserName = user.NormalizedUserName,
                    PasswordHash = passwordHashSalt,
                    PasswordHashSalt = passwordHashSalt,
                    Role = user.Role,
                    Salt = Convert.ToBase64String(salt),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    TriboEquipe = user.TriboEquipe,
                    UserName = user.UserName,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled
                };

                _context.Users.Add(usuario);
                _context.SaveChanges();

                return Identidade.Success;
            }
            catch (Exception e)
            {
                return Identidade.Failed(new IdentidadeError() { Code = e.HResult.ToString(), Description = e.Message });
            }
        }

        public Task<ResultDynamic> GetUserByCpfCnpj(string cpfCnpj)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDynamic> GetUserByEmail(string email)
        {
            Usuario? user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);

            return new ResultDynamic
            {
                User = user,
                SignInResultado = user == null ? SignInResultado.NotAllowed : SignInResultado.Success
            };
        }

        public Task<ResultDynamic> GetUserByNome(string nome)
        {
            throw new NotImplementedException();
        }

        public Task<Identidade> UpdateAcessLock(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<Result<Paginacao<UsuarioListDto>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var lista = await _context.Users
                    .Include(x => x.TriboEquipe)
                    .Select(x => new UsuarioListDto
                    {
                        Cpf = x.Cpf,
                        Email = x.Email,
                        Nome = x.Nome,
                        TriboEquipe = x.TriboEquipe,
                        UserName = x.UserName,
                        PhoneNumber = x.PhoneNumber,
                    })
                    .ToListAsync();

                return Result<Paginacao<UsuarioListDto>>.Sucesso(new Paginacao<UsuarioListDto>
                {
                    Dados = lista,
                    Count = lista.Count,
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<UsuarioListDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }
    }
}
