using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace BrewingCoder.NetCore.NHibernateUowRepo
{
    public static class NHibernateFactoryManager
    {
        private static Lazy<ISessionFactory?> _lazy = null;

        public static ISessionFactory? Instance(FluentConfiguration config)
        {
            if (_lazy != null && _lazy.IsValueCreated) return _lazy.Value;

            _lazy = new Lazy<ISessionFactory?>(config.BuildSessionFactory);
            return _lazy.Value;
        }

        public static ISessionFactory? Instance()
        {
            return (_lazy != null && _lazy.IsValueCreated) ? _lazy.Value : null;
        }
    }
}
