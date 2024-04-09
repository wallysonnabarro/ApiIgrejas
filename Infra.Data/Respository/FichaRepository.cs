using Azure;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Util;
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

        public async Task<Result<FichaPagamento>> GetFichasInscricoes(FichaParametros parametros)
        {
            try
            {
                if (parametros.Tipo == 1)//Voluntários
                {
                    return await Voluntarios(parametros);
                }
                else //Consumidores
                {
                    return await Consumidores(parametros);
                }
            }
            catch (Exception ex)
            {
                return Result<FichaPagamento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        private async Task<int> Count(int id)
        {
            return await _db.FichasLider.Include(x => x.Evento).Where(x => x.Evento.Id == id).CountAsync();
        }

        private async Task<Result<FichaPagamento>> Voluntarios(FichaParametros parametros, bool transferencia = false)
        {
            try
            {
                var page = parametros.Skip == 0 ? 0 : parametros.Skip - 1;
                var lista = await _db.FichasLider.Include(x => x.Evento).Where(x => x.Evento.Id == parametros.Evento && (transferencia == true ? x.Confirmacao == 0 : x.Confirmacao != 3))
                    .Select(x => new ListaInscricoes
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Confirmacao = x.Confirmacao,
                        Sexo = x.Sexo,
                    }).ToListAsync();

                foreach (var item in lista)
                {
                    var pagamento = await _db.Pagamentos
                        .Include(x => x.Voluntario)
                        .Include(x => x.Evento)
                        .FirstOrDefaultAsync(x => x.Voluntario!.Id == item.Id && x.Evento.Id == parametros.Evento);

                    item.Pago = pagamento == null ? 0 : (pagamento!.Pix ?? 0) + (pagamento.Credito ?? 0) + (pagamento.CreditoParcelado ?? 0) + (pagamento.Debito ?? 0) + (pagamento.Dinheiro ?? 0) + (pagamento.Descontar ?? 0);
                    item.Receber = pagamento == null ? 0 : pagamento.Receber;
                }

                var fichas = new FichaPagamento
                {
                    Count = lista.Count,
                    Dados = lista.Skip(page * parametros.PageSize).Take(parametros.PageSize).ToList(),
                    PageIndex = parametros.Skip == 0 ? 1 : parametros.Skip,
                    PageSize = parametros.PageSize,
                    Feminino = new DadosCard
                    {
                        TotalConfirmado = lista.Where(x => x.Sexo == 2 && x.Confirmacao == 1).Count(),
                        TotalNaoConfirmado = lista.Where(x => x.Sexo == 2 && x.Confirmacao == 0).Count(),
                        TotalGeral = lista.Where(x => x.Sexo == 2).Count()
                    },
                    Masculino = new DadosCard
                    {
                        TotalConfirmado = lista.Where(x => x.Sexo == 1 && x.Confirmacao == 1).Count(),
                        TotalNaoConfirmado = lista.Where(x => x.Sexo == 1 && x.Confirmacao == 0).Count(),
                        TotalGeral = lista.Where(x => x.Sexo == 1).Count()
                    },
                };

                return Result<FichaPagamento>.Sucesso(fichas);
            }
            catch (Exception ex)
            {
                return Result<FichaPagamento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        private async Task<Result<FichaPagamento>> Consumidores(FichaParametros parametros, bool transferencia = false)
        {
            try
            {
                var page = parametros.Skip == 0 ? 0 : parametros.Skip - 1;
                var lista = await _db.FichasConectados.Include(x => x.Evento).Where(x => x.Evento.Id == parametros.Evento && (transferencia == true ? x.Confirmacao == 0 : x.Confirmacao != 3))
                    .Select(x => new ListaInscricoes
                    {
                        Id = x.Id,
                        Nome = x.Nome,
                        Confirmacao = x.Confirmacao,
                        Sexo = x.Sexo,
                        Idade = new Datas().ConvertData(x.Nascimento),
                    }).ToListAsync();

                foreach (var item in lista)
                {
                    var pagamento = await _db.Pagamentos
                        .Include(x => x.FichaConsumidor)
                        .Include(x => x.Evento)
                        .FirstOrDefaultAsync(x => x.FichaConsumidor!.Id == item.Id && x.Evento.Id == parametros.Evento);

                    item.Pago = pagamento == null ? 0 : (pagamento!.Pix ?? 0) + (pagamento.Credito ?? 0) + (pagamento.CreditoParcelado ?? 0) + (pagamento.Debito ?? 0) + (pagamento.Dinheiro ?? 0) + (pagamento.Descontar ?? 0);
                    item.Receber = pagamento == null ? 0 : pagamento.Receber;
                }

                var fichas = new FichaPagamento
                {
                    Count = lista.Count,
                    Dados = lista.Skip(page * parametros.PageSize).Take(parametros.PageSize).ToList(),
                    PageIndex = parametros.Skip == 0 ? 1 : parametros.Skip,
                    PageSize = parametros.PageSize,
                    Feminino = new DadosCard
                    {
                        TotalConfirmado = lista.Where(x => x.Sexo == 2 && x.Confirmacao == 1).Count(),
                        TotalNaoConfirmado = lista.Where(x => x.Sexo == 2 && x.Confirmacao == 0).Count(),
                        TotalGeral = lista.Where(x => x.Sexo == 2).Count()
                    },
                    Masculino = new DadosCard
                    {
                        TotalConfirmado = lista.Where(x => x.Sexo == 1 && x.Confirmacao == 1).Count(),
                        TotalNaoConfirmado = lista.Where(x => x.Sexo == 1 && x.Confirmacao == 0).Count(),
                        TotalGeral = lista.Where(x => x.Sexo == 1).Count()
                    },
                };

                return Result<FichaPagamento>.Sucesso(fichas);
            }
            catch (Exception ex)
            {
                return Result<FichaPagamento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<bool>> NovoConectado(FichaConectadoDto dto)
        {
            try
            {
                var existe = await _db.FichasConectados.Include(s => s.Evento).FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
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
                        Confirmacao = 0,
                        Tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Tribo)!,
                        Evento = await _db.Eventos.FirstOrDefaultAsync(s => s.Id == dto.Siao)!
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
                var existe = await _db.FichasLider.Include(s => s.Evento).FirstOrDefaultAsync(x => x.Nome.Equals(dto.Nome));
                if (existe != null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Inscrição já foi registrado.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    var ficha = new FichaLider
                    {
                        Nome = dto.Nome,
                        Sexo = dto.Sexo,
                        Confirmacao = 0,
                        Tribo = await _db.TribosEquipes.FirstOrDefaultAsync(x => x.Id == dto.Tribo),
                        Evento = await _db.Eventos.FirstOrDefaultAsync(s => s.Id == dto.Siao)!,
                        Area = await _db.AreasSet.FirstOrDefaultAsync(s => s.Id == dto.Area)
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

        public async Task<Result<FichaPagamento>> GetFichasInscricoesNaoconfirmados(FichaParametros parametros)
        {
            try
            {
                if (parametros.Tipo == 1)//Voluntários
                {
                    return await Voluntarios(parametros, true);
                }
                else //Consumidores
                {
                    return await Consumidores(parametros, true);
                }
            }
            catch (Exception ex)
            {
                return Result<FichaPagamento>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }
    }
}
