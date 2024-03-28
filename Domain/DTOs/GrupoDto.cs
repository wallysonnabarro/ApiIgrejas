using Domain.Dominio.menus;

namespace Domain.DTOs
{
    public class GrupoDto
    {
        public required string Grupo { get; set; }
        public required string NomeUsuarioCriacao { get; set; }
        public required string usuario { get; set; }
        public required List<MenusDto> MenusDto { get; set; }
        public List<SubmenuDto>? SubmenuDto { get; set; }
    }
}
