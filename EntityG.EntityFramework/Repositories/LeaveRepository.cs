using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntityG.EntityFramework.Repositories
{
    public class LeaveRepository : BaseRepository<Leave>, ILeaveRepository
    {
        public LeaveRepository(DbContext context) : base(context)
        {
        }
    }
}
