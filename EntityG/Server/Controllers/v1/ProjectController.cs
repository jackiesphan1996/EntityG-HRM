using System;
using System.Threading.Tasks;
using EntityG.BusinessLogic.Caching.Interfaces.Proxies;
using EntityG.BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EntityG.Server.Controllers.v1
{
    public class ProjectController : BaseApiController<ProjectController>
    {
        private readonly IProjectService _projectService;
        private readonly IProjectProxy _projectProxy;

        public ProjectController(IProjectService projectService, IProjectProxy projectProxy)
        {
            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
            _projectProxy = projectProxy ?? throw new ArgumentNullException(nameof(projectProxy));
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

            return Ok(await _projectProxy.GetAllAsync());
        }
    }
}
