using EntityG.BusinessLogic.Services.Interfaces;
using EntityG.Contracts.Requests.Employees;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class EmployeeController : BaseApiController<EmployeeController>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(int page, int pageSize, string search)
        {
            Logger.LogInformation($"Calling api GetAllEmployees page: {page}, pageSize : {pageSize} & search : {search}");

            return Ok(await _employeeService.GetAllAsync(page, pageSize, search));
        }

        [HttpGet]
        [Route("lookup")]
        public async Task<IActionResult> GetAllEmployees()
        {
            Logger.LogInformation($"Calling api - GetAllEmployees Lookup");

            return Ok(await _employeeService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateEmployeeDto request)
        {
            Logger.LogInformation($"Calling api Create Employee with Json : {JsonConvert.SerializeObject(request)}");

            await _employeeService.CreateAsync(request);

            Logger.LogInformation("Created successfully.");

            return Ok(await Result.SuccessAsync("Created successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateEmployeeDto request)
        {
            Logger.LogInformation($"Calling api Update Employee with Json : {JsonConvert.SerializeObject(request)}");

            await _employeeService.UpdateAsync(request);

            Logger.LogInformation("Updated successfully.");

            return Ok(await Result.SuccessAsync("Updated successfully."));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Detail([FromRoute] int id)
        {
            Logger.LogInformation($"Calling api Get Employee detail with Id : {id}");

            return Ok(await _employeeService.GetByIdAsync(id));
        }
    }
}