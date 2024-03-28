using Domain.Dominio.menus.Submenus;

namespace Domain.Dominio.menus
{
    public class Grupos : IEntity
    {
        public int Id { get; set; }
        public required string Grupo { get; set; }
        public DateTime DtCriacao { get; set; }
        public required string NomeUsuarioCriacao { get; set; }
        public int IdUsuarioCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public string? JustificativaModificacao { get; set; }

        //Navegação
        public virtual required ICollection<Menus> GroupMenus { get; set; }
        public virtual ICollection<Submenu>? GruposSubmenus { get; set; }
        public virtual ICollection<Role>? GroupRoles { get; set; }
    }
}
