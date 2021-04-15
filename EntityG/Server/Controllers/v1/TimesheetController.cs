using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Requests.Timesheets;
using EntityG.EntityFramework.Entities;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class TimesheetController : BaseApiController<TimesheetController>
    {
        private readonly ITimesheetService _timesheetService;
        private readonly IEmployeeService _employeeService;

        public TimesheetController(ITimesheetService timesheetService, IEmployeeService employeeService)
        {
            _timesheetService = timesheetService ?? throw new ArgumentNullException(nameof(timesheetService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTimesheets(int page, int pageSize, string search)
        {
            Logger.LogInformation($"Calling api GetAllTimesheets page: {page}, pageSize : {pageSize} & search : {search}");

            Employee currentEmployee = await _employeeService.GetByUserId(CurrentUser.UserId);

            if (currentEmployee == null)
            {
                throw new ArgumentNullException(nameof(currentEmployee), "Employee info for current user does not exist.");
            }

            return Ok(await _timesheetService.GetAllAsync(currentEmployee.Id,page, pageSize, search));
        }

        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> GetAllTimesheets(int year, int month)
        {
            Logger.LogInformation($"Calling api GetAllTimesheets year: {year}, month : {month}");

            Employee currentEmployee = await _employeeService.GetByUserId(CurrentUser.UserId);

            if (currentEmployee == null)
            {
                throw new ArgumentNullException(nameof(currentEmployee), "Employee info for current user does not exist.");
            }

            return Ok(await _timesheetService.GetAllAsync(currentEmployee.Id, year, month));
        }

        [HttpGet]
        [Route("range/{fromDate}/{toDate}")]
        [Authorize]
        public async Task<IActionResult> GetAllTimesheets([FromRoute] DateTime fromDate, [FromRoute] DateTime toDate)
        {
            Logger.LogInformation($"Calling api GetAllTimesheets FromDate: {fromDate}, ToDate : {toDate}");

            Employee currentEmployee = await _employeeService.GetByUserId(CurrentUser.UserId);

            if (currentEmployee == null)
            {
                throw new ArgumentNullException(nameof(currentEmployee), "Employee info for current user does not exist.");
            }

            return Ok(await _timesheetService.GetAllAsync(currentEmployee.Id, fromDate, toDate));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTimesheetRequest request)
        {
            Logger.LogInformation($"Calling api Create Employee with Json : {JsonConvert.SerializeObject(request)}");

            await _timesheetService.CreateAsync(request);

            Logger.LogInformation("Created successfully.");

            return Ok(await Result.SuccessAsync("Created successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateTimesheetRequest request)
        {
            Logger.LogInformation($"Calling api Create Timesheet with Json : {JsonConvert.SerializeObject(request)}");

            await _timesheetService.UpdateAsync(request);

            Logger.LogInformation("Updated successfully.");

            return Ok(await Result.SuccessAsync("Updated successfully."));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _timesheetService.DeleteAsync(id);

            return Ok(await Result.SuccessAsync("Deleted successfully."));
        }
    }
}
