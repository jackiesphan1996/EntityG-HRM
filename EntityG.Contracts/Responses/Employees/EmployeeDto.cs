using System;

namespace EntityG.Contracts.Responses.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string EmployeeIdNumber { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string SupervisorId { get; set; }
        public string Supervisor { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal UnpaidLeavePerDay { get; set; }
        public string AccountTitle { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string SystemUserId { get; set; }
        public string SystemUserEmail { get; set; }
    }
}
