using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityG.EntityFramework.Entities.Identity;

namespace EntityG.EntityFramework.Entities
{
    public class Employee : AuditableEntity
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName => $"{LastName} {FirstName}";
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string EmployeeIdNumber { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string SupervisorId { get; set; }
        public Employee Supervisor { get; set; }
        [Required]
        public decimal BasicSalary { get; set; }
        [Required]
        public decimal UnpaidLeavePerDay { get; set; }
        [Required]
        public string AccountTitle { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        public string SystemUserId { get; set; }       
        public ApplicationUser SystemUser { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
    }
}
