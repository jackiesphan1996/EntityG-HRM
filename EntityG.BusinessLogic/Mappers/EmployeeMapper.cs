using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class EmployeeMapper
    {
        public static Employee ToEntity(CreateEmployeeDto item)
        {
            return new Employee
            {
                EmployeeIdNumber = item.EmployeeIdNumber,
                Phone = item.Phone,
                AccountNumber = item.AccountNumber,
                Email = item.Email,
                AccountTitle = item.AccountTitle,
                Address1 = item.Address1,
                Address2 = item.Address2,
                BankName = item.BankName,
                BasicSalary = item.BasicSalary,
                City = item.City,
                Country = item.Country,
                DateOfBirth = item.DateOfBirth,
                DepartmentId = item.DepartmentId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Gender = item.Gender,
                JoiningDate = item.JoiningDate,
                LeavingDate = item.LeavingDate,
                MaritalStatus = item.MaritalStatus,
                PlaceOfBirth = item.PlaceOfBirth,
                StateProvince = item.StateProvince,
                UnpaidLeavePerDay = item.UnpaidLeavePerDay,
                SupervisorId = item.SupervisorId,
                SystemUserId = item.SystemUserId,
                ZipCode = item.ZipCode
            };
        }

        public static LookupDto ToLookup(Employee item)
        {
            return new LookupDto
            {
                Id = item.Id.ToString(),
                Value = item.EmployeeIdNumber
            };
        }

        public static EmployeeDto Map(Employee item)
        {
            return new EmployeeDto
            {
                Id = item.Id,
                EmployeeIdNumber = item.EmployeeIdNumber,
                Phone = item.Phone,
                AccountNumber = item.AccountNumber,
                Email = item.Email,
                AccountTitle = item.AccountTitle,
                Address1 = item.Address1,
                Address2 = item.Address2,
                BankName = item.BankName,
                BasicSalary = item.BasicSalary,
                City = item.City,
                Country = item.Country,
                DateOfBirth = item.DateOfBirth,
                Department = item.Department?.Name,
                DepartmentId = item.DepartmentId,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Gender = item.Gender,
                JoiningDate = item.JoiningDate,
                LeavingDate = item.LeavingDate,
                MaritalStatus = item.MaritalStatus,
                PlaceOfBirth = item.PlaceOfBirth,
                StateProvince = item.StateProvince,
                UnpaidLeavePerDay = item.UnpaidLeavePerDay,
                Supervisor = item.SystemUser?.FullName,
                SupervisorId = item.SupervisorId,
                SystemUserId = item.SystemUserId,
                ZipCode = item.ZipCode,
                SystemUserEmail = item.SystemUser?.Email
            };
        }
    }
}
