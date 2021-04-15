using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntityG.EntityFramework.Repositories
{
    public class TimesheetRepository : BaseRepository<Timesheet>, ITimesheetRepository
    {
        public TimesheetRepository(DbContext context) : base(context)
        {
        }
    }
}
