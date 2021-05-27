using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Requests.Department;
using EntityG.Contracts.Responses.Department;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IDepartmentRepository departmentRepository, IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<PagingResult<DepartmentDto>> GetAllAsync(int page, int pageSize, string search)
        {
            Expression<Func<Department, bool>> filter = null;

            if (!string.IsNullOrEmpty(search))
            {
                filter = x => x.Name.Contains(search);
            }

            IOrderedQueryable<Department> OrderBy(IQueryable<Department> x)
            {
                return x.OrderByDescending(y => y.CreatedOn);
            }

            int skip = (page - 1) * pageSize;

            int totalData = await _departmentRepository.CountAsync(filter);

            List<Department> departments = await _departmentRepository.GetAllAsync(filter:filter,  orderBy:OrderBy, countSkip:skip, countTake: pageSize);

            return new PagingResult<DepartmentDto>
            {
                Data = departments.Select(DepartmentMapper.Map).ToList(),
                TotalCount = totalData
            };
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var employees = await _departmentRepository.GetAllAsync(orderBy: x => x.OrderBy(y => y.Name));

            var result = employees.Select(x => new LookupDto
            {
                Id = x.Id.ToString(),
                Value = x.Name
            }).ToList();

            return await Result<List<LookupDto>>.SuccessAsync(result);
        }

        public async Task<int> CreateAsync(CreateDepartmentDto request)
        {
            if ( await _departmentRepository.AnyAsync(x => x.Name.Equals(request.Name)))
            {
                throw new ValidationException($"Error: {request.Name } already exist");
            }

            var department = new Department
            {
                Name = request.Name,
                Description = request.Description
            };

            _departmentRepository.Add(department);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UpdateDepartmentDto request)
        {
            Department editDepartment = editDepartment = await _departmentRepository.FirstOrDefaultAsync(x => x.Id.Equals(request.Id));
            Department existDepartment = await _departmentRepository.FirstOrDefaultAsync(x => x.Name.Equals(request.Name));

            if (editDepartment == null)
            {
                throw new ValidationException($"Error: Id {request.Id} does not exist.");
            }

            if (existDepartment != null && editDepartment.Id != existDepartment.Id)
            {
                throw new ValidationException($"Error: {request.Name } already exist");
            }

            editDepartment.Name = request.Name;
            editDepartment.Description = request.Description;
            _departmentRepository.Update(editDepartment);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var removedData =
                await _departmentRepository.FirstOrDefaultAsync(filter: x => x.Id == id, includes:y => y.Include(z => z.Employees));

            if (removedData == null)
            {
                throw new ValidationException($"Error: ID {id} does not exist.");
            }

            if (removedData.Employees.Any())
            {
                throw new ValidationException($"Error: Can not delete department '{removedData.Name}' because there are {removedData.Employees.Count} people in this department.");
            }

            _departmentRepository.Remove(removedData);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
