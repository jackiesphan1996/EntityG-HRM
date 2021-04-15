using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;

namespace EntityG.Client.Pages.UserRoles
{
    public partial class UserRoles
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Description { get; set; }

        private List<UserRoleModel> UserRolesList { get; set; } = new List<UserRoleModel>();

        private UserResponse CurrentUser { get; set; } = new UserResponse();

        protected override async Task OnInitializedAsync()
        {
            var result = await _userManager.GetAsync(Id);
            if (result.Succeeded)
            {
                CurrentUser = result.Data;
                if (CurrentUser != null)
                {
                    Title = $"{CurrentUser.FirstName} {CurrentUser.LastName}";
                    Description = $"Manage {CurrentUser.FirstName} {CurrentUser.LastName}'s Roles";
                    var response = await _userManager.GetRolesAsync(CurrentUser.Id);
                    UserRolesList = response.Data.UserRoles;
                }
            }
        }
        private async Task SaveAsync()
        {
            var request = new UpdateUserRolesRequest()
            {
                UserId = Id,
                UserRoles = UserRolesList
            };
            var result = await _userManager.UpdateRolesAsync(request);
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo($"/identity/user-roles/{Id}");
                await _message.Success(result.Messages[0]);
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    await _message.Error(error);
                }
            }
        }
    }
}