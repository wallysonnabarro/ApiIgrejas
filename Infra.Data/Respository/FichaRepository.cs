using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class FichaRepository : IFichaRepository
    {
        private readonly ContextDb _db;

        public FichaRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<bool>> NovoConectado(FichaConectadoDto dto)
        {
            try
            {
                var existe = await _db.FichasConectados.Include(s => s.Siao).FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
                if (existe != null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = $"{existe.Id}", mensagem = "Inscrição já foi registrado.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    var ficha = new FichaConectado
                    {
                        Nome = dto.Nome,
                        Cep = dto.Cep,
                        Email = dto.Email,
                        Endereco = dto.Endereco,
                        Lider = dto.Lider,
                        Telefone = dto.Telefone,
                        ContatoEmergencial = dto.ContatoEmergencial,
                        Crianca = dto.Crianca,
                        Cuidados = dto.Cuidados,
                        DescricaoCuidados = dto.DescricaoCuidados,
                        EstadoCivil = dto.EstadoCivil,
                        Idade = dto.Idade,
                        Nascimento = dto.Nascimento,
                        Sexo = dto.Sexo,
                        Tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Tribo)!,
                        Siao = await _db.Siaos.FirstOrDefaultAsync(s => s.Id == dto.Siao)!
                    };

                    _db.Add(ficha);
                    await _db.SaveChangesAsync();

                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<bool>> NovoLider(FichaLiderDto dto)
        {
            try
            {
                var existe = await _db.FichasLider.Include(s => s.Siao).FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
                if (existe == null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Inscrição já foi registrado.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    var ficha = new FichaLider
                    {
                        Nome = dto.Nome,
                        Sexo = dto.Sexo,
                        Tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Tribo),
                        Siao = await _db.Siaos.FirstOrDefaultAsync(s => s.Id == dto.Siao)!
                    };

                    _db.Add(ficha);
                    await _db.SaveChangesAsync();

                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }
    }
}
