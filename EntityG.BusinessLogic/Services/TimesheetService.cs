using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Requests.Timesheets;
using EntityG.Contracts.Responses.Timesheets;
using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Entities.Identity;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Constants.Role;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
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
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TimesheetService> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimesheetService(
            ITimesheetRepository timesheetRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<TimesheetService> logger,
            ICurrentUserService currentUserService,
            IEmployeeRepository employeeRepository,
            UserManager<ApplicationUser> userManager)
        {
            _timesheetRepository = timesheetRepository ?? throw new ArgumentNullException(nameof(timesheetRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<PagingResult<TimesheetDto>> GetAllAsync(int employeeId, int pageIndex, int pageSize, string search)
        {
            try
            {
                Expression<Func<Timesheet, bool>> filter = x => x.EmployeeId == employeeId;

                IOrderedQueryable<Timesheet> OrderBy(IQueryable<Timesheet> x)
                {
                    return x.OrderByDescending(y => y.Date);
                }

                IIncludableQueryable<Timesheet, object> Includes(IQueryable<Timesheet> x)
                {
                    return x.Include(y => y.Employee).Include(y => y.Project);
                }

                int skip = (pageIndex - 1) * pageSize;

                int totalData = await _timesheetRepository.CountAsync(filter);

                List<Timesheet> timesheets = await _timesheetRepository.GetAllAsync(filter, Includes, OrderBy, skip, pageSize, true);

                return new PagingResult<TimesheetDto>
                {
                    Data = timesheets.Select(TimesheetMapper.Map).ToList(),
                    TotalCount = totalData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when EmployeeService.GetAllAsync");
                throw;
            }
        }

        public async Task<int> CreateAsync(CreateTimesheetRequest request)
        {
            var timesheet = new Timesheet
            {
                ProjectId = request.ProjectId,
                Activity = request.Activity,
                HourRate = request.HourRate,
                Hours = request.Hours,
                Comment = request.Comment,
                Date = request.Date
            };

            if (request.EmployeeId == 0)
            {
                var currentEmployee = await _employeeRepository.FirstOrDefaultAsync(x => x.SystemUserId.Equals(_currentUserService.UserId));
                timesheet.EmployeeId = currentEmployee.Id;
            }

            _timesheetRepository.Add(timesheet);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(UpdateTimesheetRequest request)
        {
            var timesheet = await _timesheetRepository.GetByIdAsync(request.Id);
            timesheet.ProjectId = request.ProjectId;
            timesheet.HourRate = request.HourRate;
            timesheet.Hours = request.Hours;
            timesheet.Activity = request.Activity;
            timesheet.Date = request.Date;
            timesheet.Comment = request.Comment;

            //if (request.EmployeeId == 0)
            //{
            //    var currentEmployee = await _employeeRepository.FirstOrDefaultAsync(x => x.SystemUserId.Equals(_currentUserService.UserId));
            //    timesheet.EmployeeId = currentEmployee.Id;
            //}

            _timesheetRepository.Update(timesheet);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var timesheet = await _timesheetRepository.FirstOrDefaultAsync(x => x.Id == id, x => x.Include(y => y.Employee));
            
            if (timesheet == null)
            {
                throw new ValidationException($"Error : TimesheetId {id} does not exist");
            }

            if (!timesheet.Employee.SystemUserId.Equals(_currentUserService.UserId))
            {
                var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
                if ( !await _userManager.IsInRoleAsync(currentUser, RoleConstant.AdministratorRole))
                {
                    throw new ValidationException($"Error : Dont have permissions to do it.");
                }
            }

            _timesheetRepository.Remove(timesheet);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IResult<List<TimesheetDto>>> GetAllAsync(int employeeId, int year, int month)
        {
            Expression<Func<Timesheet, bool>> filter = x => x.EmployeeId == employeeId && x.Date.Year == year && x.Date.Month == month;

            IIncludableQueryable<Timesheet, object> Includes(IQueryable<Timesheet> x)
            {
                return x.Include(y => y.Employee).Include(y => y.Project);
            }

            List<Timesheet> timesheets = await _timesheetRepository.GetAllAsync(filter: filter, includes: Includes, disableTracking: true);

            return await Result<List<TimesheetDto>>.SuccessAsync(timesheets.Select(TimesheetMapper.Map).ToList());
        }

        public async Task<IResult<List<TimesheetDto>>> GetAllAsync(int employeeId, DateTime fromDate, DateTime toDate)
        {
            Expression<Func<Timesheet, bool>> filter = x => x.EmployeeId == employeeId && x.Date.Date <= toDate.Date && x.Date >= fromDate.Date;

            IIncludableQueryable<Timesheet, object> Includes(IQueryable<Timesheet> x)
            {
                return x.Include(y => y.Employee).Include(y => y.Project);
            }

            List<Timesheet> timesheets = await _timesheetRepository.GetAllAsync(filter: filter, includes: Includes, disableTracking: true);

            return await Result<List<TimesheetDto>>.SuccessAsync(timesheets.Select(TimesheetMapper.Map).ToList());
        }
    }
}
