using AutoMapper;
using Domain.Command;
using Domain.Dominio.menus;
using Domain.DTOs;

namespace Domain.Mappers
{
    public class GruposProfile : Profile
    {
        public GruposProfile()
        {
            ///<summary>
            /// De GrupoDto para Grupo
            /// </summary>
            CreateMap<GrupoDto, Grupos>()
                .ForMember(d => d.Grupo, opt => opt.MapFrom(src => src.Grupo))
                .ForMember(d => d.NomeUsuarioCriacao, opt => opt.MapFrom(src => src.NomeUsuarioCriacao));
        }
    }
}
