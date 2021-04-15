using System.Threading.Tasks;
using EntityG.Client.Models;
using EntityG.Client.Services;
using Microsoft.AspNetCore.Components;

namespace EntityG.Client.Pages.Dashboard.Workplace
{
    public partial class Index
    {
        private readonly EditableLink[] _links =
        {
            new EditableLink {Title = "Operation one", Href = ""},
            new EditableLink {Title = "Operation two", Href = ""},
            new EditableLink {Title = "Operation three", Href = ""},
            new EditableLink {Title = "Operation four", Href = ""},
            new EditableLink {Title = "Operation five", Href = ""},
            new EditableLink {Title = "Operation six", Href = ""}
        };

        private ActivitiesType[] _activities = { };
        private NoticeType[] _projectNotice = { };

        [Inject] public IProjectService ProjectService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _projectNotice = await ProjectService.GetProjectNoticeAsync();
            _activities = await ProjectService.GetActivitiesAsync();
        }
    }
}