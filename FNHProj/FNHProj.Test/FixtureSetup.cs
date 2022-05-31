using BrewingCoder.NetCore.NHibernateUowRepo;
using BrewingCoder.netCore.NHibernateUowRepo.Tests.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace FNHProj.Test
{
    [SetUpFixture]
    public class FixtureSetup
    {
        public static ISessionFactory Factory;


        public static FluentConfiguration Configuration
        {
            get
            {
                return Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(c => c
                        .Server("va-test-1")
                        .Database("bcUowIntegrationTests")
                        .Username("dev")
                        .Password("dev")
                    ))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Foo>())
                    .ExposeConfiguration(c =>
                    {
                        var su = new SchemaUpdate(c);
                        su.Execute(false, true);
                    });
            }
        }


        [OneTimeSetUp]
        public void Setup()
        {
            Factory = NHibernateFactoryManager.Instance(Configuration);
        }



        [OneTimeTearDown]
        public void TearDown()
        {
            Factory?.Dispose();
        }
    }
}