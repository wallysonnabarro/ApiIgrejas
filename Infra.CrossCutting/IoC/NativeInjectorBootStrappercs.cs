using Domain.Mappers;
using Infra.Data.Interfaces;
using Infra.Data.Respository;
using Microsoft.Extensions.DependencyInjection;
using Service.Interface;
using Service.Services;

namespace Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrappercs
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //scoped repository
            services.AddScoped<IContratoRepository, ContratoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IRoleRepository, RolerRepository>();

            //scoped services
            services.AddScoped<IAuthenticationServices, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthorization, Authorization>();

            //AutoMapper
            services.AddAutoMapper(typeof(ContratoProfile));
            services.AddAutoMapper(typeof(UsuarioProfile));
        }
    }
}
