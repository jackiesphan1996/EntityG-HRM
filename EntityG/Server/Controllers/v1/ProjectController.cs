using System;
using System.Threading.Tasks;
using EntityG.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EntityG.Server.Controllers.v1
{
    public class ProjectController : BaseApiController<ProjectController>
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects(int page, int pageSize, string search)
        {
            Logger.LogInformation($"Calling api - GetAllProjects page : {page}, pageSize : {pageSize} & search = '{search}'");

            return Ok(await _projectService.GetAllAsync(page, pageSize, search));
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProjects ()
        {
            Logger.LogInformation($"Calling api - GetAllProjects Lookup");

            return Ok(await _projectService.GetAllAsync());
        }
    }
}
