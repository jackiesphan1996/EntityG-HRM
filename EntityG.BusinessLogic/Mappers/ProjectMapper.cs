using System.Linq;
using EntityG.Contracts.Responses.Projects;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class ProjectMapper
    {
        public static ProjectDto Map(Project item)
        {
            return new ProjectDto
            {
                Id = item.Id,
                Name = item.Name,
                StartedOn = item.CreatedOn,
                CreatedOn = item.CreatedOn,
                Description = "",
                Employees = item.ProjectEmployees?.Select(ProjectEmployeeMapper.Map).ToList()
            };
        }

        public static LookupDto ToLookup(Project item)
        {
            return new LookupDto
            {
                Id = item.Id.ToString(),
                Value = item.Name
            };
        }
    }
}
