using Domain.Dominio.menus.Submenus;

namespace Domain.Dominio.menus
{
    public class Menus : IEntity
    {
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Route { get; set; }

        //Navegação
        public virtual ICollection<GroupMenu> MenuGroups { get; set; }
        public virtual ICollection<MenuSubmenu> SubmenusMenu { get; set; }
    }
}
