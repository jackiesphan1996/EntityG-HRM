using EntityG.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class LeaveTypeController : BaseApiController<LeaveTypeController>
    {
        private readonly ILeaveTypeService _leaveTypeService;
        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            _leaveTypeService = leaveTypeService ?? throw new ArgumentNullException(nameof(leaveTypeService));
        }

        [HttpGet]
        [Route("lookup")]
        public async Task<IActionResult> GetAllLeaveTypes()
        {
            Logger.LogInformation($"Calling api - GetAllLeaveTypes Lookup");

            return Ok(await _leaveTypeService.GetAllAsync());
        }
    }
}
