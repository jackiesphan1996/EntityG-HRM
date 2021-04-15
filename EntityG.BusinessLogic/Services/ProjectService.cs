using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Mappers;
using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Requests.Projects;
using EntityG.Contracts.Responses.Projects;
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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            IProjectRepository projectRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork, 
            ILogger<ProjectService> logger)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagingResult<ProjectDto>> GetAllAsync(int page, int pageSize, string search)
        {
            try
            {
                Expression<Func<Project, bool>> filter = null;

                if (!string.IsNullOrEmpty(search))
                {
                    filter = x => x.Name.Contains(search);
                }

                IOrderedQueryable<Project> OrderBy(IQueryable<Project> x)
                {
                    return x.OrderByDescending(y => y.CreatedOn);
                }

                IIncludableQueryable<Project, object> Includes(IQueryable<Project> x)
                {
                    return x.Include(x => x.ProjectEmployees);
                }

                int skip = (page - 1) * pageSize;

                int totalData = await _projectRepository.CountAsync(filter);

                List<Project> projects = await _projectRepository.GetAllAsync(filter, Includes, OrderBy, skip, pageSize, true);

                return new PagingResult<ProjectDto>
                {
                    Data = projects.Select(ProjectMapper.Map).ToList(),
                    TotalCount = totalData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when EmployeeService.GetAllAsync with paging.");
                throw;
            }
        }

        public async Task<int> CreateAsync(CreateProjectRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            if (_projectRepository.Any(x => x.Name.Equals(request.Name))) throw new ValidationException($"Error : {request.Name} already exits.");

            var employeeIds = request.EmployeeIds;

            var project = new Project
            {
                Name = request.Name
            };

            if (employeeIds != null & employeeIds.Any())
            {
                employeeIds = employeeIds.Distinct().ToList();

                if (_employeeRepository.Count(x => employeeIds.Contains(x.Id)) != employeeIds.Count)
                {
                    throw new ValidationException("Error : Some employees do not exist.");
                }
            }

            project.ProjectEmployees = employeeIds.Select(x => new ProjectEmployee {EmployeeId = x}).ToList();

             _projectRepository.Add(project);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync(orderBy: order => order.OrderBy(x => x.Name));

            var projectLookups = projects.Select(ProjectMapper.ToLookup).ToList();

            return await Result<List<LookupDto>>.SuccessAsync(projectLookups);
        }
    }
}
