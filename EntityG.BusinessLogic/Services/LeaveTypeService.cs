using EntityG.BusinessLogic.Mappers;
using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Interfaces;
using EntityG.EntityFramework.Interfaces.Repositories;
using EntityG.Shared.Wrapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LeaveTypeService> _logger;

        public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository, IUnitOfWork unitOfWork, ILogger<LeaveTypeService> logger)
        {
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var leaveTypes = await _leaveTypeRepository.GetAllAsync(orderBy: order => order.OrderBy(x => x.Name));

            return await Result<List<LookupDto>>.SuccessAsync(leaveTypes.Select(LeaveTypeMapper.ToLookup).ToList());
        }
    }
}
