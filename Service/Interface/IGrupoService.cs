using Domain.DTOs;

namespace Service.Interface
{
    public interface IGrupoService
    {
        bool Validacao(GrupoDto dto);
        bool Validacao(GrupoAtualizarDto dto);
    }
}
