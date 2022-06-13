using System;
using FFNHProj.Domain;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;

namespace FNHProj.Test
{
    public static class NHibernateFactoryManager
    {
        //private static Lazy<ISessionFactory?> _lazy = null;

        //public static ISessionFactory? Instance(FluentConfiguration config)
        //{
        //    if (_lazy != null && _lazy.IsValueCreated) return _lazy.Value;

        //    _lazy = new Lazy<ISessionFactory?>(config.BuildSessionFactory);
        //    return _lazy.Value;
        //}

        //public static ISessionFactory? Instance()
        //{
        //    return (_lazy != null && _lazy.IsValueCreated) ? _lazy.Value : null;
        //}

        #region Private Fields

        private static ISessionFactory _sessionFactory;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Creates <c>ISession</c>s.
        /// </summary>
        public static ISessionFactory SessionFactoryInstance
        {
            get { return _sessionFactory ?? (_sessionFactory = CreateSessionFactory()); }
        }

        /// <summary>
        /// Allows the application to specify properties and mapping documents to be used when creating a <see cref="T:NHibernate.ISessionFactory"/>.
        /// </summary>
        public static NHibernate.Cfg.Configuration Configuration { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Open a new NHibenate Session
        /// </summary>
        /// <returns>A new ISession</returns>
        public static ISession OpenSession()
        {
            var session = SessionFactoryInstance.OpenSession();

            return session;
        }

        /// <summary>
        /// Open a new stateless NHibernate Session
        /// </summary>
        /// <returns>Stateless NHibernate Session</returns>
        public static IStatelessSession OpenStatelessSession()
        {
            var session = SessionFactoryInstance.OpenStatelessSession();

            return session;
        }

        #endregion Public Methods

        #region Private Methods

        private static ISessionFactory CreateSessionFactory()
        {
            if (Configuration == null)
            {
                Configuration = new Configuration();

                Configuration.BeforeBindMapping += OnBeforeBindMapping;

                // FluentNHibernate Configuration API for configuring NHibernate
                Configuration = Fluently.Configure(Configuration)
                    .Database(
                            MsSqlConfiguration.MsSql7
                                .ConnectionString("SomeConnectionStringName")
                                .UseReflectionOptimizer()
                                .AdoNetBatchSize(100))
                    .ExposeConfiguration(
                            x =>
                            {
                                // Increase the timeout for long running queries
                                x.SetProperty("command_timeout", "600");

                                // Allows you to have non-virtual and non-public methods in your entities
                                x.SetProperty("use_proxy_validator", "false");
                            })
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<EntityBase>())
                    .BuildConfiguration();
            }

            var sessionFactory = Configuration.BuildSessionFactory();

            return sessionFactory;
        }

        private static void OnBeforeBindMapping(object sender, BindMappingEventArgs bindMappingEventArgs)
        {
            // Force using the fully qualified type name instead of just the class name.
            // This will get rid of any duplicate mapping/class name issues.
            bindMappingEventArgs.Mapping.autoimport = false;
        }

        #endregion Private Methods
    }
}

