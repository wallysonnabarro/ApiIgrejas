namespace Domain.Dominio.menus.Submenus
{
    public class Submenu
    {
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Route { get; set; }

        //navegação
        public virtual ICollection<Menus>? MenuSubmenus { get; set; }
    }
}
