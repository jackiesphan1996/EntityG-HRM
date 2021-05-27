using EntityG.BusinessLogic.Interfaces.Services;
using EntityG.Contracts.Requests.Department;
using EntityG.Shared.Wrapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EntityG.Server.Controllers.v1
{
    public class DepartmentController : BaseApiController<DepartmentController>
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService ?? throw new ArgumentNullException(nameof(departmentService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int page, int pageSize, string search)
        {
            return Ok(await _departmentService.GetAllAsync(page, pageSize,search));
        }

        [HttpGet]
        [Route("lookup")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _departmentService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDepartmentDto request)
        {
            await _departmentService.CreateAsync(request);

            return Ok(await Result.SuccessAsync("Created successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDepartmentDto request)
        {
            await _departmentService.UpdateAsync(request);

            return Ok(await Result.SuccessAsync("Updated successfully."));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            await _departmentService.DeleteAsync(id);

            return Ok(await Result.SuccessAsync("Deleted successfully."));
        }
    }
}
