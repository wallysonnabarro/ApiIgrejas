using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;

namespace Domain.Mappers
{
    public class PagamentosProfile : Profile
    {
        public PagamentosProfile()
        {
            ///<summary>
            /// De ItemPagamentoSaidaDto para PagamentoSaida
            /// </summary>
            CreateMap<ItemPagamentoSaidaDto, PagamentoSaida>()
                .ForMember(d => d.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(d => d.Tipo, opt => opt.MapFrom(src => src.Tipo))
                .ForMember(d => d.FormaPagamento, opt => opt.MapFrom(src => src.FormaPagamento))
                .ForMember(d => d.TipoNome, opt => opt.MapFrom(src => src.TipoNome))
                .ForMember(d => d.Valor, opt => opt.MapFrom(src => src.Valor));
        }
    }
}
