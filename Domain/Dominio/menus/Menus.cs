namespace Domain.Dominio.menus
{
    public class Menus : IEntity
    {
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Route { get; set; }

        //Navegação
        public virtual ICollection<Grupos>? MenuGroups { get; set; }
    }
}
