using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Pro.Layout;
using EntityG.Client.Models;
using EntityG.Client.Services;
using Microsoft.AspNetCore.Components;

namespace EntityG.Client.Pages.Profile
{
    public partial class Advanced
    {
        private readonly IList<TabPaneItem> _tabList = new List<TabPaneItem>
        {
            new TabPaneItem {Key = "detail", Tab = "详情"},
            new TabPaneItem {Key = "rules", Tab = "规则"}
        };

        private AdvancedProfileData _data = new AdvancedProfileData();

        [Inject] protected IProfileService ProfileService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _data = await ProfileService.GetAdvancedAsync();
        }
    }
}