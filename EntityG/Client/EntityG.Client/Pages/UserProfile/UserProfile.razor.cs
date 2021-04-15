using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;

namespace EntityG.Client.Pages.UserProfile
{
    public partial class UserProfile
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Title { get; set; }
        private UserResponse _user = new UserResponse();
        private bool Active { get; set; }
        public IEnumerable<string> Errors { get; set; }

        private async Task ToggleUserStatus()
        {
            var request = new ToggleUserStatusRequest { ActivateUser = Active, UserId = Id };
            var result = await _userManager.ToggleUserStatusAsync(request);
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo("/identity/users");
                await _message.Success("Updated User Status.");
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    await _message.Error(error);
                }
            }
        }

        [Parameter]
        public string ImageDataUrl { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var userId = Id;
            var result = await _userManager.GetAsync(userId);
            if (result.Succeeded)
            {
                var user = result.Data;
                if (user != null)
                {
                    _user = user;
                    Active = user.IsActive;
                    var data = await _accountManager.GetProfilePictureAsync(userId);
                    if (data.Succeeded)
                    {
                        ImageDataUrl = data.Data;
                    }
                }
            }
            else
            {
                
            }
        }
    }
}