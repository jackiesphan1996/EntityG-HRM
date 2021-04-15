using AntDesign;
using EntityG.Contracts.Responses.Projects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.Projects
{
    public partial class Projects
    {
        private List<ProjectDto> _projects = new List<ProjectDto>();

        private ProjectDto _project = new ProjectDto();

        private int Page { get; set; } = 1;

        private int PageSize { get; set; } = 10;

        private int TotalCount { get; set; }

        private string SearchText { get; set; } = "";

        private bool IsLoading { get; set; }

        private bool ProjectDialogVisible { get; set; }

        private string ProjectDialogTitle { get; set; }

        private Form<ProjectDto> ProjectDialogForm { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllProjectsWithPaging();
        }

        private async Task GetAllProjectsWithPaging()
        {
            IsLoading = true;
            var response = await _projectManager.GetAllAsync(Page, PageSize, SearchText);
            if (response.Succeeded)
            {
                _projects = response.Data;
                TotalCount = response.TotalCount;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }

            IsLoading = false;
        }

        private async Task HandlePageIndexChanged(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetAllProjectsWithPaging();
        }

        private async Task HandlePageSizeChange(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetAllProjectsWithPaging();
        }

        private void CreateProject()
        {
            _project = new ProjectDto();
            ProjectDialogVisible = true;
            ProjectDialogTitle = "Create Project";
        }

        private void HandleProjectDialogCancel()
        {
            ProjectDialogVisible = false;
        }

        private async Task HandleProjectDialogOk()
        {
            if (this.ProjectDialogForm.Validate())
            {
                
            }
        }

    }
}