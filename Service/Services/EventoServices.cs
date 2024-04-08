using Service.Interface;
using Service.Utilitarios;

namespace Service.Services
{
    public class EventoServices : IEventoServices
    {
        public async Task<string> GerarTokenEvento()
        {
            return await TokenEvento.GenerateToken();
        }
    }
}
