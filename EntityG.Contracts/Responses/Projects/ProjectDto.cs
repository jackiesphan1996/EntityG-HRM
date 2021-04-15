using System;
using System.Collections.Generic;

namespace EntityG.Contracts.Responses.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<ProjectEmployeeDto> Employees { get; set; }
    }
}
