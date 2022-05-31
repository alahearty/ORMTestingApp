using BrewingCoder.NetCore.NHibernateUowRepo.Contracts;
using FNHProj.Infrastructure.Contracts;

namespace FNHProj.Infrastructure.UnitOfWork
{
    public abstract class EntityBase : IEntity
    {
        public virtual uint Id { get; set; }
    }
}
