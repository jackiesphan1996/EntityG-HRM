using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Mappers;
using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Common.Helpers;
using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var employees = await _employeeRepository.GetAllAsync(orderBy: order => order.OrderBy(x => x.EmployeeIdNumber));

            return await Result<List<LookupDto>>.SuccessAsync(employees.Select(EmployeeMapper.ToLookup).ToList());
        }

        public async Task<PagingResult<EmployeeDto>> GetAllAsync(int pageIndex, int pageSize, string search)
        {
            try
            {
                Expression<Func<Employee, bool>> filter = null;

                if (!string.IsNullOrEmpty(search))
                {
                    filter = x => x.AccountNumber.Contains(search)
                                  || x.Department.Name.Contains(search)
                                  || x.EmployeeIdNumber.Contains(search)
                                  || x.Email.Contains(search)
                                  || x.FirstName.Contains(search)
                                  || x.LastName.Contains(search)
                                  || x.Phone.Contains(search);
                }

                IOrderedQueryable<Employee> OrderBy(IQueryable<Employee> x)
                {
                    return x.OrderByDescending(y => y.CreatedOn);
                }

                IIncludableQueryable<Employee, object> Includes(IQueryable<Employee> x)
                {
                    return x.Include(y => y.Department).Include(y => y.SystemUser);
                }

                int skip = (pageIndex - 1) * pageSize;

                int totalData = await _employeeRepository.CountAsync(filter);

                List<Employee> employees = await _employeeRepository.GetAllAsync(filter, Includes, OrderBy, skip, pageSize, true);

                return new PagingResult<EmployeeDto>
                {
                    Data = employees.Select(EmployeeMapper.Map).ToList(),
                    TotalCount = totalData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError( "Error when EmployeeService.GetAllAsync");
                throw;
            }
            
        }

        public async Task<Employee> GetByUserId(string id)
        {
            return await _employeeRepository.FirstOrDefaultAsync(x => x.SystemUserId.Equals(id));
        }

        public async Task<int> CreateAsync(CreateEmployeeDto request)
        {
            Require.IsNotNull(request);

            try
            {
                Employee employee = EmployeeMapper.ToEntity(request) ?? throw new ArgumentNullException(nameof(request));

                if (await _employeeRepository.AnyAsync(x => x.EmployeeIdNumber.Equals(request.EmployeeIdNumber)))
                {
                    throw new ValidationException($"EmployeeIdNumber : {request.EmployeeIdNumber} already exists.");
                }

                if (await _employeeRepository.AnyAsync(x => x.Email.Equals(request.Email)))
                {
                    throw new ValidationException($"Email : {request.Email} already exists.");
                }

                _employeeRepository.Add(employee);

                return await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error when EmployeeService.CreateAsync");
                throw;
            }
            
        }

        public async Task<int> UpdateAsync(UpdateEmployeeDto request)
        {
            Require.IsNotNull(request);

            try
            {
                var editEmployee = await _employeeRepository.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));

                if (editEmployee == null)
                {
                    throw new ValidationException($"Error : Id {request.Id} does not exist");
                }

                //duplicate Employee Number ID is not allowed
                Employee empNumber = await _employeeRepository.FirstOrDefaultAsync(x => x.EmployeeIdNumber.Equals(request.EmployeeIdNumber), disableTracking: true);
                if (empNumber != null && editEmployee.EmployeeIdNumber != request.EmployeeIdNumber && !String.IsNullOrEmpty(request.EmployeeIdNumber))
                {
                    throw new ValidationException($"Error: Employee Id Number - {request.EmployeeIdNumber} Can Not Duplicate.");
                }

                //duplicate System user account is not allowed
                Employee empSystem = await _employeeRepository.FirstOrDefaultAsync(x => x.SystemUserId.Equals(request.SystemUserId), disableTracking: true);
                if (empSystem != null && editEmployee.SystemUserId != request.SystemUserId && !String.IsNullOrEmpty(request.SystemUserId))
                {
                    throw new ValidationException("Error: Application System User Account Already Been Used. " + request.SystemUserId);
                }

                editEmployee.FirstName = request.FirstName;
                editEmployee.LastName = request.LastName;
                editEmployee.Gender = request.Gender;
                editEmployee.DateOfBirth = request.DateOfBirth;
                editEmployee.PlaceOfBirth = request.PlaceOfBirth;
                editEmployee.MaritalStatus = request.MaritalStatus;
                editEmployee.Email = request.Email;
                editEmployee.Phone = request.Phone;
                editEmployee.Address1 = request.Address1;
                editEmployee.Address2 = request.Address2;
                editEmployee.City = request.City;
                editEmployee.StateProvince = request.StateProvince;
                editEmployee.ZipCode = request.ZipCode;
                editEmployee.Country = request.Country;
                editEmployee.EmployeeIdNumber = request.EmployeeIdNumber;
                editEmployee.DepartmentId = request.DepartmentId;
                editEmployee.JoiningDate = request.JoiningDate;
                editEmployee.LeavingDate = request.LeavingDate;
                editEmployee.SupervisorId = request.SupervisorId;
                editEmployee.BasicSalary = request.BasicSalary;
                editEmployee.UnpaidLeavePerDay = request.UnpaidLeavePerDay;
                editEmployee.AccountTitle = request.AccountTitle;
                editEmployee.BankName = request.BankName;
                editEmployee.AccountNumber = request.AccountNumber;
                editEmployee.SystemUserId = request.SystemUserId;
                _employeeRepository.Update(editEmployee);

                return await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when calling EmployeeService.UpdateAsync");
                throw;
            }
            
        }

        public async Task<IResult<EmployeeDto>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _employeeRepository.GetByIdAsync(id);
                if (result == null)
                {
                    throw new ValidationException($"Id : {id} does not exist.");
                }

                return await Result<EmployeeDto>.SuccessAsync(EmployeeMapper.Map(result));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, $"Error when calling EmployeeService.GetByIdAsync({id})");
                throw;
            }
            
        }
    }
}
