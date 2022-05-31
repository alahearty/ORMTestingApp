using BrewingCoder.NetCore.NHibernateUowRepo;

namespace BrewingCoder.netCore.NHibernateUowRepo.Tests.Model
{
    public class Foo : EntityBase
    {
        public virtual string FooName { get; set; }
    }
}
