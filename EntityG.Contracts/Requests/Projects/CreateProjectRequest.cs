using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityG.Contracts.Requests.Projects
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> EmployeeIds { get; set; }
    }
}
