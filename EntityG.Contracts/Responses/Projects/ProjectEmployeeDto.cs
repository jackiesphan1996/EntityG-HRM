using System;

namespace EntityG.Contracts.Responses.Projects
{
    public class ProjectEmployeeDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeIdNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public int ProjectId { get; set; }
    }
}
