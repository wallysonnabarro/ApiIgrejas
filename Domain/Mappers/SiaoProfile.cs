using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;

namespace Domain.Mappers
{
    public class SiaoProfile : Profile
    {
        public SiaoProfile()
        {
            ///<summary>
            /// De SiaoNovoDto para Siao
            /// </summary>
            CreateMap<SiaoNovoDto, Evento>()
                .ForMember(d => d.Nome, opt => opt.MapFrom(src => src.Evento))
                .ForMember(d => d.Coordenadores, opt => opt.MapFrom(src => src.Coordenadores))
                .ForMember(d => d.Inicio, opt => opt.MapFrom(src => src.Inicio))
                .ForMember(d => d.Termino, opt => opt.MapFrom(src => src.Termino))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(d => d.Descricao, opt => opt.MapFrom(src => src.Descricao));
        }
    }
}
