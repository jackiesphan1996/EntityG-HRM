using EntityG.Contracts.Responses.Projects;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class ProjectEmployeeMapper
    {
        public static ProjectEmployeeDto Map(ProjectEmployee item)
        {
            return new ProjectEmployeeDto
            {
                Id = item.Id,
                ProjectId = item.ProjectId,
                EmployeeId = item.EmployeeId,
                EmployeeIdNumber = item.Employee?.EmployeeIdNumber
            };
        }
    }
}
