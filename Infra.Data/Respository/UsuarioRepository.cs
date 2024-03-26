using AutoMapper;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
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

        public Task<ResultDynamic> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResultDynamic> GetUserByNome(string nome)
        {
            throw new NotImplementedException();
        }

        public Task<Identidade> UpdateAcessLock(int id)
        {
            throw new NotImplementedException();
        }
    }
}
