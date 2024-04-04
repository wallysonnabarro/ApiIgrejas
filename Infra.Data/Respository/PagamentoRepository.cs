using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly ContextDb _db;

        public PagamentoRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<bool>> Cancelar(int id, string EmailUser)
        {
            try
            {
                var existe = await _db.Pagamentos.FirstOrDefaultAsync(x => x.Id == id);
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
                        existe.Cancelado = 0;
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

        public async Task<Result<bool>> Confirmar(PagamentoDto dto, string EmailUser)
        {
            try
            {
                var pg = new Pagamento
                {
                    Siao = await _db.Siaos.FirstOrDefaultAsync(x => x.Id == dto.Siao),
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
                    .Include(x => x.Siao)
                    .FirstOrDefaultAsync(x => (tipo == 1 ? x.Voluntario!.Id == idTransferidor : x.FichaConsumidor!.Id == idTransferidor)
                                        && x.Siao.Id == siao);

                var user = await _db.Users.FirstOrDefaultAsync(x => x.UserName.Equals(EmailUser));

                if (user == null) return Result<bool>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Usuário inválido.", ocorrencia = "", versao = "" } });
                else
                {
                    if (tipo == 1)
                    {
                        var ficha = await _db.FichasLider.FirstOrDefaultAsync(x => x.Id == idRecebedor);
                        pagamentoPrimario!.Voluntario = ficha;
                    }
                    else
                    {
                        var ficha = await _db.FichasConectados.FirstOrDefaultAsync(x => x.Id == idRecebedor);
                        pagamentoPrimario!.FichaConsumidor = ficha;
                    }
                    pagamentoPrimario.Usuario = user;
                    pagamentoPrimario.DtTransferencia = DateTime.Now;
                    pagamentoPrimario.ObsTransferencia = obs;
                    pagamentoPrimario.Transferido = 1;

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
