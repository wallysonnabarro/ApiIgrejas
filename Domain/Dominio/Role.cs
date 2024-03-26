using Domain.Dominio.menus;

namespace Domain.Dominio
{
    public class Role : IEntity
    {
        public int Id { get; set; }
        public required string Nome { get; set; }

        public virtual ICollection<RoleGroup>? RoleGroups { get; set; }
    }
}
