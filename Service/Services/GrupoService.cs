using Domain.DTOs;
using Service.Interface;

namespace Service.Services
{
    public class GrupoService : IGrupoService
    {
        public bool Validacao(GrupoDto dto)
        {
            var validado = true;

            if (dto.Grupo == null || dto.Grupo.Equals(""))
            {
                return validado = false;
            }
            else if (dto.NomeUsuarioCriacao == null || dto.NomeUsuarioCriacao == "")
            {
                return validado = false;
            }
            else if (dto.NomeUsuarioCriacao == null || dto.NomeUsuarioCriacao.Equals(""))
            {
                return validado = false;
            }

            return validado;
        }

        public bool Validacao(GrupoAtualizarDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
