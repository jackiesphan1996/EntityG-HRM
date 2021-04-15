using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class LeaveController : BaseApiController<LeaveController>
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeService _employeeService;

        public LeaveController(ILeaveService leaveService, IEmployeeService employeeService)
        {
            _leaveService = leaveService ?? throw new ArgumentNullException(nameof(leaveService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllLeaves(int page, int pageSize, DateTime fromDate, DateTime toDate, bool? isApproved)
        {
            Employee currentEmployee = await _employeeService.GetByUserId(CurrentUser.UserId);

            if (currentEmployee == null)
            {
                throw new ArgumentNullException(nameof(currentEmployee), "Employee info for current user does not exist.");
            }

            return Ok(await _leaveService.GetAllLeaves( page, pageSize, currentEmployee.Id, fromDate, toDate, isApproved));
        }
    }
}
