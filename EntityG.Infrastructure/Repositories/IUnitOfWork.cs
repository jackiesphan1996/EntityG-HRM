using System;
using System.Threading;
using System.Threading.Tasks;
using EntityG.Domain.Contracts;

namespace EntityG.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<T> Repository<T>() where T : AuditableEntity;

        Task<int> Commit(CancellationToken cancellationToken);

        Task Rollback();
    }
}