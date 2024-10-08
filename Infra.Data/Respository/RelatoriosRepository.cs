﻿using Domain.Dominio;
using Domain.DTOs;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class RelatoriosRepository : IRelatoriosRepository
    {
        private readonly ContextDb _db;

        public RelatoriosRepository(ContextDb db)
        {
            _db = db;
        }

        public async Task<Result<FichasDto<List<CheckInReports>>>> GetByConectados(ParametrosConectados dto)
        {
            try
            {
                List<CheckInReports> lista = await _db.FichasConectados
                        .Include(x => x.Evento)
                        .Include(x => x.Tribo)
                        .Select(x => new CheckInReports
                        {
                            Nome = x.Nome,
                            Tribo = x.Tribo.Nome,
                            Sexo = x.Sexo,
                            Confirmado = x.Confirmacao
                        })
                        .Where(x => x.Confirmado == 1 && x.Sexo == dto.Sexo)
                        .OrderBy(x => x.Nome)
                        .ToListAsync();

                if (lista == null || lista.Count == 0) return Result<FichasDto<List<CheckInReports>>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Nenhum conectado confirmado.", ocorrencia = "", versao = "" } });

                var dados = new FichasDto<List<CheckInReports>>
                {
                    Dados = lista
                };

                return Result<FichasDto<List<CheckInReports>>>.Sucesso(dados);
            }
            catch (Exception e)
            {
                return Result<FichasDto<List<CheckInReports>>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = e.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<DadosRelatorio<List<CheckInReports>>>> GetByIdHomens(ParametrosEvento dto)
        {
            try
            {
                List<CheckInReports> lista = await _db.FichasLider
                        .Include(x => x.Area)
                        .Include(x => x.Evento)
                        .Include(x => x.Tribo)
                        .Select(x => new CheckInReports
                        {
                            Area = x.Area.Nome,
                            Nome = x.Nome,
                            Tribo = x.Tribo.Nome,
                            Sexo = x.Sexo,
                            Confirmado = x.Confirmacao
                        })
                        .Where(x => x.Confirmado == 1)
                        .OrderBy(x => x.Area)
                        .ToListAsync();

                if (lista == null || lista.Count == 0) return Result<DadosRelatorio<List<CheckInReports>>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = "Nenhum voluntário confirmado.", ocorrencia = "", versao = "" } });


                string diretorioBase = AppDomain.CurrentDomain.BaseDirectory;

                var caminhoArquivo = Path.Combine("imgs", "siao.png");

                string caminhoCompleto = Path.Combine(diretorioBase, caminhoArquivo);
                byte[] imagemBytes = File.ReadAllBytes(caminhoCompleto);

                var dados = new DadosRelatorio<List<CheckInReports>>
                {
                    Dados = lista
                };

                return Result<DadosRelatorio<List<CheckInReports>>>.Sucesso(dados);
            }
            catch (Exception e)
            {
                return Result<DadosRelatorio<List<CheckInReports>>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = e.Message, ocorrencia = "", versao = "" } });
            }
        }

        public async Task<Result<DadosReaderRelatorio>> NovoConectadoReader(DadosReaderRelatorioDto dto)
        {
            try
            {
                if (await _db.DadosReaderRelatorios.AnyAsync(x => x.Tipo == dto.TipoRelatorio))
                {
                    var atualizar = await _db.DadosReaderRelatorios.FirstAsync(x => x.Tipo == dto.TipoRelatorio);
                    atualizar.Tipo = dto.TipoRelatorio;
                    atualizar.Titulo = dto.Titulo;
                    atualizar.Subtitulo = dto.Subtitulo;

                    await _db.SaveChangesAsync();
                    return Result<DadosReaderRelatorio>.Sucesso(atualizar);
                }
                else
                {
                    var novo = new DadosReaderRelatorio
                    {
                        Subtitulo = dto.Subtitulo,
                        Titulo = dto.Titulo,
                        Imagem = dto.Imagem,
                        Tipo = dto.TipoRelatorio
                    };

                    _db.Add(novo);
                    await _db.SaveChangesAsync();
                    return Result<DadosReaderRelatorio>.Sucesso(novo);
                }
            }
            catch (Exception ex)
            {
                return Result<DadosReaderRelatorio>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "" } });
            }
        }
    }
}
