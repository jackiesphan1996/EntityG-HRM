using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityG.EntityFramework.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
    }
}
