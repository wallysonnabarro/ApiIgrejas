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
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            services.AddScoped<ITriboEquipesRepository, TriboEquipesRepository>();
            services.AddScoped<ISiaoRepository, SiaoRepository>();
            services.AddScoped<IAreasRepository, AreasRepository>();
            services.AddScoped<IFichaRepository, FichaRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IRelatoriosRepository, RelatoriosRepository>();

            //scoped services
            services.AddScoped<IAuthenticationServices, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthorization, Authorization>();
            services.AddScoped<IEventoServices, EventoServices>();
            services.AddScoped<IRelatorioServices, RelatorioServices>();
            services.AddScoped<IRenderType, RenderTypeServices>();

            //AutoMapper
            services.AddAutoMapper(typeof(ContratoProfile));
            services.AddAutoMapper(typeof(UsuarioProfile));
        }
    }
}
