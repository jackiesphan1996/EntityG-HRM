using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.BusinessLogic.Mappers;
using EntityG.Contracts.Responses.Leaves;
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
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LeaveService> _logger;

        public LeaveService(ILeaveRepository leaveRepository, IUnitOfWork unitOfWork, ILogger<LeaveService> logger)
        {
            _leaveRepository = leaveRepository ?? throw new ArgumentNullException(nameof(leaveRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagingResult<LeaveDto>> GetAllLeaves(int page, int pageSize, int employeeId, DateTime fromDate, DateTime toDate, bool? isApproved)
        {
            try
            {
                Expression<Func<Leave, bool>> filter = x => x.EmployeeId == employeeId 
                                                            && fromDate.Date <= x.FromDate
                                                            && x.ToDate <= toDate
                                                            && (isApproved == null || x.IsApproved == isApproved);

                IOrderedQueryable<Leave> OrderBy(IQueryable<Leave> x)
                {
                    return x.OrderByDescending(y => y.FromDate);
                }

                IIncludableQueryable<Leave, object> Includes(IQueryable<Leave> x)
                {
                    return x.Include(y => y.Employee).Include(y => y.LeaveType);
                }

                int skip = (page - 1) * pageSize;


                int totalData = await _leaveRepository.CountAsync(filter);

                List<Leave> leaves = new List<Leave>();

                if (totalData > 0)
                {
                    leaves = await _leaveRepository.GetAllAsync(filter, Includes, OrderBy, skip, pageSize, true);
                }

                return new PagingResult<LeaveDto>
                {
                    Data = leaves.Select(LeaveMapper.Map).ToList(),
                    TotalCount = totalData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get all leaves");
                throw;
            }
        }
    }
}
