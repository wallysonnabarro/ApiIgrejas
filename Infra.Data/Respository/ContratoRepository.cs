using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;
using Domain.Mappers;
using Infra.Data.Context;
using Infra.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Respository
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly Mapper _mapper;
        private readonly ContextDb _contextDb;

        public ContratoRepository(ContextDb contextDb)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ContratoProfile>());
            _mapper = new Mapper(config);
            _contextDb = contextDb;
        }

        public async Task<Result<ContratoDto>> NovoContrato(ContratoDto dto)
        {
            var contrato = _mapper.Map<ContratoCommand>(dto);

            if (!contrato.IsValid())
            {
                List<Erros> erros = new List<Erros>();

                foreach (var item in contrato.ValidationResult.Errors)
                {
                    erros.Add(new Erros()
                    {
                        codigo = item.ErrorCode,
                        mensagem = item.ErrorMessage,
                        ocorrencia = item.PropertyName,
                        versao = ""
                    });
                }

                return Result<ContratoDto>.Failed(erros);
            }

            var existe = await _contextDb.Contratos.FirstOrDefaultAsync(x => x.CNPJ.Equals(contrato.CNPJ));

            if (existe != null)
            {
                List<Erros> erros = new List<Erros>();
                erros.Add(new Erros()
                {
                    codigo = $"{existe.Id}",
                    mensagem = "Existe uma empresa registrada com este CNPJ.",
                    ocorrencia = "Novo",
                    versao = "V1"
                });
                return Result<ContratoDto>.Failed(erros);
            }

            var novo = _mapper.Map<Contrato>(dto);

            _contextDb.Add(novo);
            await _contextDb.SaveChangesAsync();

            return Result<ContratoDto>.Success;
        }

        private async Task<int> Count()
        {
            return await _contextDb.Contratos.CountAsync();
        }

        public async Task<Result<Paginacao<Contrato>>> Paginacao(PageWrapper wrapper)
        {
            try
            {
                var page = wrapper.Skip == 0 ? 0 : wrapper.Skip - 1;

                var lista = await _contextDb.Contratos
                    .Skip(page * wrapper.PageSize)
                    .Take(wrapper.PageSize)
                    .ToListAsync();

                return Result<Paginacao<Contrato>>.Sucesso(new Paginacao<Contrato>
                {
                    Dados = lista,
                    Count = await Count(),
                    PageIndex = wrapper.Skip == 0 ? 1 : wrapper.Skip,
                    PageSize = wrapper.PageSize
                });
            }
            catch (Exception ex)
            {
                return Result<Paginacao<Contrato>>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }

        public async Task<Result<Contrato>> Update(ContratoDto dto, int id)
        {
            try
            {
                var contrato = _mapper.Map<ContratoCommand>(dto);

                if (!contrato.IsValid())
                {
                    List<Erros> erros = new List<Erros>();

                    foreach (var item in contrato.ValidationResult.Errors)
                    {
                        erros.Add(new Erros()
                        {
                            codigo = item.ErrorCode,
                            mensagem = item.ErrorMessage,
                            ocorrencia = item.PropertyName,
                            versao = ""
                        });
                    }

                    return Result<Contrato>.Failed(erros);
                }

                var existe = await _contextDb.Contratos
                    .Include(x => x.Endereco)
                    .FirstOrDefaultAsync(x => x.CNPJ.Equals(contrato.CNPJ) && x.Id == id);

                if (existe == null)
                {
                    List<Erros> erros = new List<Erros>();
                    erros.Add(new Erros()
                    {
                        codigo = $"{existe!.Id}",
                        mensagem = "Contrato não localizado.",
                        ocorrencia = "Novo",
                        versao = "V1"
                    });
                    return Result<Contrato>.Failed(erros);
                }

                existe.RazaoSocia = dto.RazaoSocia;
                existe.CNPJ = dto.CNPJ;
                existe.Telefone = dto.Telefone;
                existe.Responsavel = dto.Responsavel;
                existe.Empresa = dto.Empresa;
                existe.Status = dto.Status;
                existe.Vencimento = dto.Vencimento;
                existe.Endereco = new Endereco
                {
                    bairro = dto.Bairro,
                    Cep = dto.Cep,
                    localidade = dto.Localidade,
                    logradouro = dto.Logradouro,
                    uf = dto.Uf,
                    complemento = dto.Complemento,
                    ddd = dto.Ddd,
                    gia = dto.Gia,
                    ibge = dto.Ibge,
                    siafi = dto.Siafi
                };

                await _contextDb.SaveChangesAsync();

                return Result<Contrato>.Success;
            }
            catch (Exception ex)
            {
                return Result<Contrato>.Failed(new List<Erros> { new Erros { codigo = "", mensagem = ex.Message, ocorrencia = "", versao = "V1" } });
            }
        }
    }
}
