using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntityG.EntityFramework.Repositories
{
    public class LeaveTypeRepository : BaseRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(DbContext context) : base(context)
        {
        }
    }
}
