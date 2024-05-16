using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Collections.Generic;

namespace Infra.Data.Respository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly ContextDb _db;
        private readonly Mapper _mapper;

        public PagamentoRepository(ContextDb db)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
            _db = db;
        }

        public async Task<Result<bool>> Cancelar(PagamentoCancelarDto dto, string EmailUser)
        {
            try
            {
                var existe = await _db.Pagamentos
                    .Include(p => p.FichaConsumidor)
                    .Include(p => p.Voluntario)
                    .Include(p => p.Evento)
                    .FirstOrDefaultAsync(x => (dto.Tipo == 1 ? x.Voluntario!.Id == dto.Id : x.FichaConsumidor!.Id == dto.Id)
                        && x.Evento.Id == dto.Siao);

                if (existe == null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Pagamento não localizado.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(EmailUser));

                    if (user == null) return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
                    else
                    {
                        if (dto.Tipo == 1) existe.Voluntario!.Confirmacao = 0;
                        else existe.FichaConsumidor!.Confirmacao = 0;

                        await _db.SaveChangesAsync();

                        _db.Pagamentos.Remove(existe);
                        await _db.SaveChangesAsync();
                        return Result<bool>.Sucesso(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<PagamentoAtualizarDto>> BuscarAtualizar(PagamentoCancelarDto dto)
        {
            try
            {
                var existe = await _db.Pagamentos
                    .Include(p => p.FichaConsumidor)
                    .Include(p => p.Voluntario)
                    .Include(p => p.Evento)
                    .Select(x => new PagamentoAtualizarDto
                    {
                        Credito = x.Credito,
                        CreditoParcelado = x.CreditoParcelado,
                        DataRegistro = x.DataRegistro,
                        Debito = x.Debito,
                        Descontar = x.Descontar,
                        Desistente = x.Desistente,
                        Dinheiro = x.Dinheiro,
                        FichaConsumidor = dto.Tipo == 2 ? x.FichaConsumidor!.Id : 0,
                        Voluntario = dto.Tipo == 1 ? x.Voluntario!.Id : 0,
                        Observacao = x.Observacao,
                        Parcelas = x.Parcelas,
                        Pix = x.Pix,
                        Receber = x.Receber,
                        Siao = dto.Siao,
                        IdPagamento = x.Id
                    })
                    .FirstOrDefaultAsync(x => (dto.Tipo == 1 ? x.Voluntario == dto.Id : x.FichaConsumidor == dto.Id)
                        && x.Siao == dto.Siao);

                if (existe == null)
                {
                    return Result<PagamentoAtualizarDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Pagamento não localizado.", ocorrencia = "", versao = "" } });
                }

                return Result<PagamentoAtualizarDto>.Sucesso(existe);
            }
            catch (Exception ex)
            {
                return Result<PagamentoAtualizarDto>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<bool>> Confirmar(PagamentoDto dto, string EmailUser)
        {
            try
            {
                var pg = new Pagamento
                {
                    Evento = await _db.Eventos.FirstOrDefaultAsync(x => x.Id == dto.Siao),
                    Usuario = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(EmailUser)),
                    Credito = dto.Credito,
                    CreditoParcelado = dto.CreditoParcelado,
                    DataRegistro = dto.DataRegistro,
                    Debito = dto.Debito,
                    Descontar = dto.Descontar,
                    Desistente = dto.Desistente,
                    Dinheiro = dto.Dinheiro,
                    FichaConsumidor = dto.FichaConsumidor == 0 ? null : await _db.FichasConectados.FirstOrDefaultAsync(x => x.Id == dto.FichaConsumidor),
                    Observacao = dto.Observacao,
                    Parcelas = dto.Parcelas,
                    Pix = dto.Pix,
                    Receber = dto.Receber,
                    Voluntario = dto.Voluntario == 0 ? null : await _db.FichasLider.FirstOrDefaultAsync(x => x.Id == dto.Voluntario)
                };

                _db.Add(pg);
                await _db.SaveChangesAsync();

                if (dto.FichaConsumidor != 0)
                {
                    var consumidor = await _db.FichasConectados.FirstOrDefaultAsync(x => x.Id == dto.FichaConsumidor);
                    consumidor!.Confirmacao = 1;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    var voluntario = await _db.FichasLider.FirstOrDefaultAsync(x => x.Id == dto.Voluntario);
                    voluntario!.Confirmacao = 1;
                    await _db.SaveChangesAsync();
                }

                return Result<bool>.Sucesso(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<bool>> Transferidor(int idRecebedor, int idTransferidor, int tipo, string EmailUser, int siao, string obs)
        {
            try
            {
                var pagamentoPrimario = await _db.Pagamentos
                    .Include(x => x.Voluntario)
                    .Include(x => x.FichaConsumidor)
                    .Include(x => x.Evento)
                    .FirstOrDefaultAsync(x => (tipo == 1 ? x.Voluntario!.Id == idTransferidor : x.FichaConsumidor!.Id == idTransferidor)
                                        && x.Evento.Id == siao);

                var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(EmailUser));

                int identificador = 0;

                if (user == null) return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
                else
                {
                    if (tipo == 1)
                    {
                        var ficha = await _db.FichasLider.FirstOrDefaultAsync(x => x.Id == idRecebedor);
                        ficha!.Confirmacao = 1;
                        identificador = pagamentoPrimario!.Voluntario!.Id;
                        pagamentoPrimario!.Voluntario = ficha;
                    }
                    else
                    {
                        var ficha = await _db.FichasConectados.FirstOrDefaultAsync(x => x.Id == idRecebedor);
                        ficha!.Confirmacao = 1;
                        identificador = pagamentoPrimario!.FichaConsumidor!.Id;
                        pagamentoPrimario!.FichaConsumidor = ficha;
                    }
                    pagamentoPrimario.Usuario = user;
                    pagamentoPrimario.DtTransferencia = DateTime.Now;
                    pagamentoPrimario.ObsTransferencia = obs;
                    pagamentoPrimario.Transferido = 1;

                    await _db.SaveChangesAsync();

                    if (tipo == 1)
                    {
                        var ficha = await _db.FichasLider.FirstOrDefaultAsync(x => x.Id == identificador);
                        ficha!.Confirmacao = 0;
                    }
                    else
                    {
                        var ficha = await _db.FichasConectados.FirstOrDefaultAsync(x => x.Id == identificador);
                        ficha!.Confirmacao = 0;
                    }

                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<bool>> Atualizar(PagamentoAtualizarDto dto)
        {
            try
            {
                var existe = await _db.Pagamentos.FirstOrDefaultAsync(x => x.Id == dto.IdPagamento);

                if (existe == null)
                {
                    return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Pagamento não localizado.", ocorrencia = "", versao = "" } });
                }
                else
                {
                    existe.Debito = dto.Debito;
                    existe.Credito = dto.Credito;
                    existe.Parcelas = dto.Parcelas;
                    existe.CreditoParcelado = dto.CreditoParcelado;
                    existe.Pix = dto.Pix;
                    existe.Descontar = dto.Descontar;
                    existe.Observacao = dto.Observacao;
                    existe.Receber = dto.Receber;
                    existe.Dinheiro = dto.Dinheiro;

                    await _db.SaveChangesAsync();
                    return Result<bool>.Sucesso(true);
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<List<PagamentosDto>>> GetPagamento(int id)
        {
            try
            {
                var evento = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);

                if (evento != null)
                {
                    var pagamentos = await _db.Pagamentos
                        .Include(e => e.Evento)
                        .Where(p => p.Evento.Id == evento.Id).ToListAsync();

                    var primeiro = pagamentos.GroupBy(g => g.Evento.Id)
                        .Select(s => new PagamentosDto
                        {
                            Tipo = 1,
                            Credito = s.Sum(x => x.Credito ?? 0),
                            Debito = s.Sum(x => x.Debito ?? 0),
                            Dinheiro = s.Sum(x => x.Dinheiro ?? 0),
                            CreditoParcelado = s.Sum(x => x.CreditoParcelado ?? 0),
                            Pix = s.Sum(x => x.Pix ?? 0),
                            Receber = s.Sum(x => x.Receber ?? 0),
                            Descontar = s.Sum(x => x.Descontar ?? 0),
                            Total = s.Sum(x => x.Credito ?? 0) + s.Sum(x => x.Debito ?? 0) + s.Sum(x => x.Dinheiro ?? 0) + s.Sum(x => x.CreditoParcelado ?? 0) + s.Sum(x => x.Pix ?? 0) + s.Sum(x => x.Receber ?? 0) + s.Sum(x => x.Descontar ?? 0)
                        });

                    var pagSaida = await _db.PagamentoSaidas
                        .Include(x => x.Evento)
                        .Where(x => x.Evento.Id == id)
                        .GroupBy(g => g.Evento.Id)
                        .Select(s => new PagamentosDto
                        {
                            Tipo = 2,
                            Dinheiro = s.Sum(x => x.FormaPagamento.Equals("Dinheiro") ? x.Valor : 0),
                            Debito = s.Sum(x => x.FormaPagamento.Equals("Débito") ? x.Valor : 0),
                            Credito = s.Sum(x => x.FormaPagamento.Equals("Crédito") ? x.Valor : 0),
                            CreditoParcelado = s.Sum(x => x.FormaPagamento.Equals("Crédito Parcelado") ? x.Valor : 0),
                            Pix = s.Sum(x => x.FormaPagamento.Equals("PIX") ? x.Valor : 0),
                            Total = s.Sum(x => x.FormaPagamento.Equals("Dinheiro") ? x.Valor : 0)
                            + s.Sum(x => x.FormaPagamento.Equals("Débito") ? x.Valor : 0)
                            + s.Sum(x => x.FormaPagamento.Equals("Crédito") ? x.Valor : 0)
                            + s.Sum(x => x.FormaPagamento.Equals("Crédito Parcelado") ? x.Valor : 0)
                            + s.Sum(x => x.FormaPagamento.Equals("PIX") ? x.Valor : 0)
                        }).ToListAsync();

                    List<PagamentosDto> lista = new List<PagamentosDto>();
                    lista.Add(primeiro.First());
                    lista.Add(pagSaida.First());

                    return Result<List<PagamentosDto>>.Sucesso(lista);
                }
                else
                {
                    return Result<List<PagamentosDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Evento não localizado.", ocorrencia = "", versao = "" } });
                }
            }
            catch (Exception ex)
            {
                return Result<List<PagamentosDto>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<string>> RegistrarListaSaida(List<ItemPagamentoSaidaDto> dto, string EmailUser, int id)
        {
            try
            {
                List<PagamentoSaida> pagamentos = new List<PagamentoSaida>();

                var evento = await _db.Eventos.FirstOrDefaultAsync(x => x.Id == id);

                foreach (var item in dto)
                {
                    PagamentoSaida saida = new()
                    {
                        Descricao = item.Descricao,
                        FormaPagamento = item.FormaPagamento,
                        Tipo = item.Tipo,
                        Valor = item.Valor,
                        TipoNome = item.TipoNome,
                        Evento = evento
                    };

                    pagamentos.Add(saida);
                }

                await _db.PagamentoSaidas.AddRangeAsync(pagamentos);

                await _db.SaveChangesAsync();

                return Result<string>.Sucesso("Lista de pagamentos registrada com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<List<ListPagamento>>> ListaPagamentoVoluntariosExcel(int id)
        {

            try
            {
                if (await _db.Pagamentos.Include(x => x.Evento).AnyAsync(x => x.Evento.Id == id))
                {
                    var lista = await _db.Pagamentos
                        .Include(x => x.Evento)
                        .Include(x => x.Voluntario)
                        .ThenInclude(v => v.Tribo)
                        .Where(x => x.Evento.Id == id && x.Voluntario!.Confirmacao == 1)
                        .Select(x => new ListPagamento
                        {
                            Siao = x.Evento.Nome,
                            Nome = x.Voluntario!.Nome,
                            Tribo = x.Voluntario.Tribo.Nome,
                            Sexo = x.Voluntario.Sexo == 1 ? "Masculino" : "Feminino",
                            dinheiro = x.Dinheiro ?? 0,
                            debito = x.Debito ?? 0,
                            credVista = x.Credito ?? 0,
                            credParcelado = x.CreditoParcelado ?? 0,
                            tedPix = x.Pix ?? 0,
                            descontar = x.Descontar ?? 0,
                            receber = x.Receber ?? 0,
                            obs = x.Observacao == null ? "" : x.Observacao
                        })
                        .OrderBy(x => x.Tribo)
                        .ToListAsync();


                    return Result<List<ListPagamento>>.Sucesso(lista);
                }
                else
                {
                    return Result<List<ListPagamento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "O evento não foi localizado.", ocorrencia = "", versao = "" } });
                }

            }
            catch (Exception ex)
            {
                return Result<List<ListPagamento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<List<ListPagamento>>> ListaPagamentoConcetadosExcel(int id)
        {
            try
            {
                if (await _db.Pagamentos.Include(x => x.Evento).AnyAsync(x => x.Evento.Id == id))
                {
                    var lista = await _db.Pagamentos
                        .Include(x => x.Evento)
                        .Include(x => x.FichaConsumidor)
                        .ThenInclude(v => v.Tribo)
                        .Where(x => x.Evento.Id == id && x.FichaConsumidor!.Confirmacao == 1)
                        .Select(x => new ListPagamento
                        {
                            Siao = x.Evento.Nome,
                            Nome = x.FichaConsumidor!.Nome,
                            Tribo = x.FichaConsumidor.Tribo.Nome,
                            Sexo = x.FichaConsumidor.Sexo == 1 ? "Masculino" : "Feminino",
                            dinheiro = x.Dinheiro ?? 0,
                            debito = x.Debito ?? 0,
                            credVista = x.Credito ?? 0,
                            credParcelado = x.CreditoParcelado ?? 0,
                            tedPix = x.Pix ?? 0,
                            descontar = x.Descontar ?? 0,
                            receber = x.Receber ?? 0,
                            obs = x.Observacao == null ? "" : x.Observacao
                        })
                        .OrderBy(x => x.Tribo)
                        .ToListAsync();


                    return Result<List<ListPagamento>>.Sucesso(lista);
                }
                else
                {
                    return Result<List<ListPagamento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "O evento não foi localizado.", ocorrencia = "", versao = "" } });
                }

            }
            catch (Exception ex)
            {
                return Result<List<ListPagamento>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<string>> RegistrarListaOfertaEvento(List<OfertaEvento> dto, string EmailUser, int id)
        {
            try
            {
                List<PagamentoOferta> pagamentos = new List<PagamentoOferta>();

                var evento = await _db.Eventos.FirstOrDefaultAsync(x => x.Id == id);

                var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(EmailUser));

                if (user == null) return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
                else
                {
                    foreach (var item in dto)
                    {
                        PagamentoOferta saida = new()
                        {
                            Forma = item.Forma,
                            Valor = item.Valor,
                            Evento = evento,
                            Usuario = user
                        };

                        pagamentos.Add(saida);
                    }

                    await _db.PagamentoOferta.AddRangeAsync(pagamentos);

                    await _db.SaveChangesAsync();

                    return Result<string>.Sucesso("Lista de pagamentos de ofertas registrado com sucesso.");
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }
    }
}
