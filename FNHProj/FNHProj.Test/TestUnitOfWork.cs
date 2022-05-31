using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BrewingCoder.NetCore.NHibernateUowRepo;
using BrewingCoder.NetCore.NHibernateUowRepo.Contracts;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace FNHProj.Test
{
    [TestFixture]
    public class TestUnitOfWork
    {
        private Mock<ISessionFactory> _mockFactory;
        private Mock<ISession> _mockSession;
        private Mock<ITransaction> _mockTransaction;

        [SetUp]
        public void Setup()
        {
            _mockFactory = new Mock<ISessionFactory>();
            _mockFactory.SetupAllProperties();
            _mockSession = new Mock<ISession>();
            _mockSession.SetupAllProperties();
            _mockTransaction = new Mock<ITransaction>();
            _mockTransaction.SetupAllProperties();

            _mockFactory.Setup(m => m.OpenSession()).Returns(_mockSession.Object);
            _mockSession.Setup(m => m.BeginTransaction(It.IsAny<IsolationLevel>())).Returns(_mockTransaction.Object);
            _mockTransaction.Setup(m => m.IsActive).Returns(true);
        }

        [Test]
        public void TestConstructor()
        {
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                Assert.IsNotNull(uow);
                Assert.IsInstanceOf<ISession>(uow.GetSession());
                _mockFactory.Verify(m => m.OpenSession(), Times.Once);
            }
            _mockSession.Verify(m => m.Dispose(), Times.Once);

        }

        [Test]
        public void TestCommitTransaction()
        {
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                uow.Commit();
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.Commit(), Times.Once);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public void TestCommitTransactionInactive()
        {
            _mockTransaction.Setup(m => m.IsActive).Returns(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                uow.Commit();
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.Commit(), Times.Never);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public void TestRollbackTransaction()
        {
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                uow.Rollback();
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.Rollback(), Times.Once);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public void TestRollbackTransactionInactive()
        {
            _mockTransaction.Setup(m => m.IsActive).Returns(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                uow.Rollback();
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.Rollback(), Times.Never);
            _mockTransaction.Verify(m => m.Commit(), Times.Never);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public async Task TestAsyncCommit()
        {
            var ct = new CancellationToken(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                await uow.CommitAsync(ct);
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.CommitAsync(ct), Times.Once);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public async Task TestAsyncCommitInactive()
        {
            _mockTransaction.Setup(m => m.IsActive).Returns(false);
            var ct = new CancellationToken(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                await uow.CommitAsync(ct);
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.CommitAsync(ct), Times.Never);
            _mockTransaction.Verify(m => m.RollbackAsync(ct), Times.Never);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public async Task TestAsyncRollback()
        {
            var ct = new CancellationToken(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                await uow.RollbackAsync(ct);
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.RollbackAsync(ct), Times.Once);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }

        [Test]
        public async Task TestAsyncRollbackInactive()
        {
            _mockTransaction.Setup(m => m.IsActive).Returns(false);
            var ct = new CancellationToken(false);
            using (IUnitOfWork uow = new UnitOfWork(_mockFactory.Object))
            {
                uow.BeginTransaction();
                await uow.RollbackAsync(ct);
            }
            _mockSession.Verify(m => m.BeginTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            _mockTransaction.Verify(m => m.RollbackAsync(ct), Times.Never);
            _mockTransaction.Verify(m => m.CommitAsync(ct), Times.Never);
            _mockSession.Verify(m => m.Dispose(), Times.Once);
            _mockTransaction.Verify(m => m.Dispose(), Times.Once);
        }


    }
}
