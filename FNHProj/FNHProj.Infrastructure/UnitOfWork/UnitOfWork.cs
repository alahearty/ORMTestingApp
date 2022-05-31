using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BrewingCoder.NetCore.NHibernateUowRepo.Contracts;
using NHibernate;

namespace BrewingCoder.NetCore.NHibernateUowRepo
{
    public class UnitOfWork : IUnitOfWork
    {
        protected ISession Session { get; set; }
        private ITransaction _transaction;

        public IsolationLevel IsolationLevel { get; set; }

        public UnitOfWork(ISessionFactory factory)
        {
            Session = factory.OpenSession();
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction(IsolationLevel);
        }

        public void Commit()
        {
            if (_transaction != null && _transaction.IsActive)
                _transaction.Commit();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction != null && _transaction.IsActive)
                await _transaction.CommitAsync(cancellationToken);
        }

        public void Rollback()
        {
            if (_transaction != null && _transaction.IsActive)
                _transaction.Rollback();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_transaction != null && _transaction.IsActive)
                await _transaction.RollbackAsync(cancellationToken);
        }

        public ISession GetSession()
        {
            return Session;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            Session?.Dispose();
        }
    }
}
