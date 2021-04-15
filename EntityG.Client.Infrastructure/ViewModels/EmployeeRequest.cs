using System;
using System.ComponentModel.DataAnnotations;

namespace EntityG.Client.Infrastructure.ViewModels
{
    public class EmployeeRequest
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
        [Required]
        public string EmployeeIdNumber { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string SupervisorId { get; set; }
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

        public EmployeeRequest()
        {
            DateOfBirth = DateTime.Now;
            JoiningDate = DateTime.Now;
        }
    }
}
