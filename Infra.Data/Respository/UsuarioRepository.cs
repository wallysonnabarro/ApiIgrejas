using AutoMapper;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats;
using Org.BouncyCastle.Crypto;
using Service.Interface;

namespace Infra.Data.Respository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ContextDb _context;
        private readonly IUserService _userServices;
        private readonly Mapper _mapper;
        private readonly IRoleRepository roleRepository;
        private readonly IContratoRepository _contratoRepository;

        public UsuarioRepository(ContextDb context, IUserService userServices, IRoleRepository roleRepository, IContratoRepository contratoRepository)
        {
            _context = context;
            _userServices = userServices;
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
            this.roleRepository = roleRepository;
            _contratoRepository = contratoRepository;
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
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    EmailConfirmed = true
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

        public async Task<SigniInUsuarioDto> GetUserByEmail(string email)
        {
            Usuario? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            var role = await _context.Roles.Include(x => x.Transacoes).FirstOrDefaultAsync(x => x.Id == user!.Role);

            return new SigniInUsuarioDto
            {
                User = user,
                Role = user == null ? null : new PerfilListaPaginadaDto
                {
                    Nome = role!.Nome,
                    Id = role.Id,
                    Transacoes = role.Transacoes
                },
                SignInResultado = user == null ? SignInResultado.NotAllowed : SignInResultado.Success
            };
        }

        public Task<ResultDynamic> GetUserByNome(string nome)
        {
            throw new NotImplementedException();
        }

        public async Task<Identidade> UpdateAcessLock(int id)
        {
            var user = await _context.Users.FirstAsync(u => u.Id == id);
            user.Tentativas = user.Tentativas == null ? 1 : user.Tentativas++;
            _context.SaveChanges();

            return Identidade.Success;
        }

        private async Task<int> Count()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<Result<Paginacao<UsuarioListDto>>> Paginacao(PageWrapper wrapper, string email)
        {
            try
            {

                var usuarioContrato = await _context.Users.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Email.Equals(email));

                if (usuarioContrato == null) return Result<Paginacao<UsuarioListDto>>.Failed(new List<Erros> { new Erros { mensagem = "Usuário não encontrado." } });

                var contrato = await getContrato(usuarioContrato.Contrato.Id);

                if (contrato.Succeeded)
                {
                    var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                    var lista = await _context.Users
                        .Include(x => x.TriboEquipe)
                        .Where(x => !x.Cpf.Equals("009.873.571-31"))
                        .Select(x => new UsuarioListDto
                        {
                            Cpf = x.Cpf,
                            Email = x.Email,
                            Nome = x.Nome,
                            TriboEquipe = x.TriboEquipe,
                            UserName = x.UserName,
                            PhoneNumber = x.PhoneNumber,
                            IdRole = x.Role
                        })
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .ToListAsync();

                    foreach (var item in lista)
                    {
                        var role = await _context.Roles
                            .Include(x => x.Contrato)
                            .Include(x => x.Transacoes)
                            .Where(x => x.Contrato.Id == contrato.Dados.Id)
                            .Select(x => new PerfilListaPaginadaDto
                            {
                                Nome = x.Nome,
                                Id = x.Id,
                                Transacoes = x.Transacoes
                            })
                            .FirstOrDefaultAsync(x => x.Id == item.IdRole);
                        item.Role = role!;
                    }

                    return Result<Paginacao<UsuarioListDto>>.Sucesso(new Paginacao<UsuarioListDto>
                    {
                        Dados = lista,
                        Count = await Count(),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<UsuarioListDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<UsuarioListDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Paginacao<UsuarioListaPaginadaDto>>> PaginacaoLista(PageWrapper wrapper, string email)
        {
            try
            {
                var usuarioContrato = await _context.Users.Include(x => x.Contrato).FirstOrDefaultAsync(x => x.Email.Equals(email));

                if (usuarioContrato == null) return Result<Paginacao<UsuarioListaPaginadaDto>>.Failed(new List<Erros> { new Erros { mensagem = "Usuário não encontrado." } });

                var contrato = await getContrato(usuarioContrato.Contrato.Id);

                if (contrato.Succeeded)
                {
                    var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                    var lista = await _context.Users
                        .Include(x => x.TriboEquipe)
                        .Where(x => !x.Cpf.Equals("009.873.571-31"))
                        .Select(x => new UsuarioListaPaginadaDto
                        {
                            Email = x.Email,
                            Nome = x.Nome,
                            Id = x.Id,
                            Tribo = x.TriboEquipe.Nome
                        })
                        .Skip(page * wrapper.PageSize)
                        .Take(wrapper.PageSize)
                        .ToListAsync();

                    return Result<Paginacao<UsuarioListaPaginadaDto>>.Sucesso(new Paginacao<UsuarioListaPaginadaDto>
                    {
                        Dados = lista,
                        Count = await Count(),
                        PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                        PageSize = wrapper.PageSize
                    });
                }
                else
                {
                    return Result<Paginacao<UsuarioListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<Paginacao<UsuarioListaPaginadaDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        private async Task<Result<Contrato>> getContrato(int email)
        {
            return await _contratoRepository.GetResult(email);
        }

        public async Task<Result<bool>> Novo(NovoUsuarioDto dto, string email)
        {
            try
            {
                var contrato = await getContrato(dto.Contrato);

                if (contrato.Succeeded)
                {
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == dto.Role);

                    if (role != null)
                    {
                        var tribo = await _context.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Tribo);

                        if (tribo != null)
                        {
                            byte[] salt = await _userServices.GenerateSalt();
                            var senha = Convert.ToBase64String(await _userServices.GeneratePasswordHash(dto.Senha, salt));
                            var usuario = new Usuario
                            {
                                Cpf = dto.Cpf,
                                Email = dto.Email,
                                NormalizedEmail = dto.Email.ToUpper(),
                                Nome = dto.Nome,
                                NormalizedUserName = dto.UserName.ToUpper(),
                                UserName = dto.UserName,
                                PasswordHash = senha,
                                PasswordHashSalt = senha,
                                Contrato = contrato.Dados,
                                Role = role.Id,
                                Salt = Convert.ToBase64String(salt),
                                SecurityStamp = Guid.NewGuid().ToString(),
                                TriboEquipe = tribo,
                                PhoneNumber = "",
                                PhoneNumberConfirmed = true,
                                TwoFactorEnabled = false,
                                EmailConfirmed = true,
                            };

                            _context.Add(usuario);
                            await _context.SaveChangesAsync();

                            return Result<bool>.Sucesso(true);
                        }
                        else return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "tribo/equipe não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "perfil não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> Editar(UsuarioEditarDto dto)
        {
            try
            {
                var contrato = await _context.Contratos.FirstOrDefaultAsync(x => x.RazaoSocia.Equals(dto.Contrato));

                if (contrato != null)
                {
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Nome == dto.Perfil);

                    if (role != null)
                    {
                        var tribo = await _context.TribosEquipes.FirstOrDefaultAsync(x => x.Nome == dto.Tribo);

                        if (tribo != null)
                        {
                            var usuario = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);
                            usuario.Cpf = dto.Cpf;
                            usuario.Contrato = contrato;
                            usuario.Role = role.Id;
                            usuario.Nome = dto.Nome;
                            usuario.UserName = dto.UserName;
                            usuario.Email = dto.Email;
                            usuario.TriboEquipe = tribo;

                            await _context.SaveChangesAsync();

                            return Result<bool>.Sucesso(true);
                        }
                        else return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "tribo/equipe não localizado.", ocorrencia = "", versao = "V1" } });
                    }
                    else return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "perfil não localizado.", ocorrencia = "", versao = "V1" } });
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Contrato não localizado.", ocorrencia = "", versao = "V1" } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<UsuarioDetalharDto>> UserDetalhe(int id)
        {
            try
            {
                var usuario = await _context.Users
                    .Include(x => x.Contrato)
                    .Include(x => x.TriboEquipe)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var detalhe = new UsuarioDetalharDto
                {
                    Contrato = usuario.Contrato.Empresa,
                    Cpf = usuario.Cpf,
                    Email = usuario.Email,
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Tribo = usuario.TriboEquipe.Nome,
                    UserName = usuario.UserName
                };

                if (detalhe != null) return Result<UsuarioDetalharDto>.Sucesso(detalhe);
                else return Result<UsuarioDetalharDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário não localizado.", ocorrencia = "", versao = "V1" } });
            }
            catch (Exception ex)
            {
                return Result<UsuarioDetalharDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<UsuarioEditarDto>> GetUsuarioEditar(int id)
        {
            try
            {
                var usuario = await _context.Users
                    .Include(x => x.Contrato)
                    .Include(x => x.TriboEquipe)
                    .FirstOrDefaultAsync(x => x.Id == id);

                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == usuario.Role);

                var detalhe = new UsuarioEditarDto
                {
                    Id = usuario.Id,
                    Contrato = usuario.Contrato.Empresa,
                    Cpf = usuario.Cpf,
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Tribo = usuario.TriboEquipe.Nome,
                    UserName = usuario.UserName,
                    Perfil = role.Nome
                };

                if (detalhe != null) return Result<UsuarioEditarDto>.Sucesso(detalhe);
                else return Result<UsuarioEditarDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário não localizado.", ocorrencia = "", versao = "V1" } });
            }
            catch (Exception ex)
            {
                return Result<UsuarioEditarDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<bool>> RedefinirSenha(LoginDTO dto)
        {
            try
            {
                var existeEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (existeEmail != null)
                {
                    byte[] salt = await _userServices.GenerateSalt();
                    var senha = Convert.ToBase64String(await _userServices.GeneratePasswordHash(dto.Senha, salt));

                    existeEmail.PasswordHash = senha;
                    existeEmail.PasswordHashSalt = senha;
                    existeEmail.Salt = Convert.ToBase64String(salt);

                    await _context.SaveChangesAsync();

                    return Result<bool>.Sucesso(true);
                }
                else
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { mensagem = "E-mail não localizado." } });
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { mensagem = ex.Message } });
            }
        }
    }
}
