using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EntityG.EntityFramework.Repositories
{
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }
    }
}
