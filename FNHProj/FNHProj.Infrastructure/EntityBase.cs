


using FNHProj.Domain.Contracts;

namespace FFNHProj.Domain
{
    public abstract class EntityBase : IEntity
    {
        public virtual uint Id { get; set; }
    }
}
