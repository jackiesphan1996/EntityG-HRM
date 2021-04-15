using EntityG.Contracts.Responses.Department;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class DepartmentMapper
    {
        public static DepartmentDto Map(Department item)
        {
            return new DepartmentDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                CreatedOn = item.CreatedOn
            };
        }
    }
}
