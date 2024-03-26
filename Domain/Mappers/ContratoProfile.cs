using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;

namespace Domain.Mappers
{
    public class ContratoProfile : Profile
    {
        public ContratoProfile()
        {
            ///<summary>
            /// De ContratoDto para ContratoCommand
            /// </summary>
            CreateMap<ContratoDto, ContratoCommand>()
                .ForMember(d => d.Empresa, opt => opt.MapFrom(src => src.Empresa))
                .ForMember(d => d.RazaoSocia, opt => opt.MapFrom(src => src.RazaoSocia))
                .ForMember(d => d.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
                .ForMember(d => d.Responsavel, opt => opt.MapFrom(src => src.Responsavel))
                .ForMember(d => d.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(d => d.Cep, opt => opt.MapFrom(src => src.Cep))
                .ForMember(d => d.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(d => d.Complemento, opt => opt.MapFrom(src => src.Complemento))
                .ForMember(d => d.Bairro, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(d => d.Localidade, opt => opt.MapFrom(src => src.Localidade))
                .ForMember(d => d.Uf, opt => opt.MapFrom(src => src.Uf))
                .ForMember(d => d.Ibge, opt => opt.MapFrom(src => src.Ibge))
                .ForMember(d => d.Gia, opt => opt.MapFrom(src => src.Gia))
                .ForMember(d => d.Ddd, opt => opt.MapFrom(src => src.Ddd))
                .ForMember(d => d.Siafi, opt => opt.MapFrom(src => src.Siafi));

            ///<summary>
            /// De ContratoDto para Contrato
            /// </summary>
            CreateMap<ContratoDto, Contrato>()
                .ForMember(d => d.Empresa, opt => opt.MapFrom(src => src.Empresa))
                .ForMember(d => d.RazaoSocia, opt => opt.MapFrom(src => src.RazaoSocia))
                .ForMember(d => d.CNPJ, opt => opt.MapFrom(src => src.CNPJ))
                .ForMember(d => d.Responsavel, opt => opt.MapFrom(src => src.Responsavel))
                .ForMember(d => d.Telefone, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(d => d.Endereco, opt => opt.MapFrom(src =>
                new Endereco
                {
                    bairro = src.Bairro,
                    Cep = src.Cep,
                    localidade = src.Localidade,
                    logradouro = src.Logradouro,
                    uf = src.Uf,
                    complemento = src.Complemento,
                    ddd = src.Ddd,
                    gia = src.Gia,
                    ibge = src.Ibge,
                    siafi = src.Siafi
                }));
        }
    }
}
