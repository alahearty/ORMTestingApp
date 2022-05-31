using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;

namespace BrewingCoder.NetCore.NHibernateUowRepo.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IsolationLevel IsolationLevel { get; set; }
        void BeginTransaction();
        
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken));

        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        ISession GetSession();
    }
}
