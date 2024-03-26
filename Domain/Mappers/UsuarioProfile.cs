using AutoMapper;
using Domain.Command;
using Domain.Dominio;
using Domain.DTOs;

namespace Domain.Mappers
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            ///<summary>
            /// De ContratoDto para ContratoCommand
            /// </summary>
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(d => d.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(d => d.Cpf, opt => opt.MapFrom(src => src.Cpf))
                .ForMember(d => d.TwoFactorEnabled, opt => opt.MapFrom(src => src.TwoFactorEnabled))
                .ForMember(d => d.Contrato, opt => opt.MapFrom(src => src.Contrato))
                .ForMember(d => d.PhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(d => d.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(d => d.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(d => d.NormalizedEmail, opt => opt.MapFrom(src => src.NormalizedEmail))
                .ForMember(d => d.NormalizedUserName, opt => opt.MapFrom(src => src.NormalizedUserName))
                .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(d => d.TriboEquipe, opt => opt.MapFrom(src => src.TriboEquipe))
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
