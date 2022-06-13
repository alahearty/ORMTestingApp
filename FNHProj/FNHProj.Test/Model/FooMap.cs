using FluentNHibernate.Mapping;
using FNHProj.Test.Model;

namespace BrewingCoder.netCore.NHibernateUowRepo.Tests.Model
{
    public class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            Table("Foos");
            Id(f => f.Id).GeneratedBy.Identity();
            Map(f => f.FooName);
        }
    }
}
