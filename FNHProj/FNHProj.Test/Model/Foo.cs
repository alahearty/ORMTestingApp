using FFNHProj.Domain;
using FNHProj.Infrastructure.UnitOfWork;

namespace FNHProj.Test.Model
{
    public class Foo : EntityBase
    {
        public virtual string FooName { get; set; }
    }
}
