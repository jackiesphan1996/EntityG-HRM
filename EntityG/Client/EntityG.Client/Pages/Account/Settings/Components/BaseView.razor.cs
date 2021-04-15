using EntityG.Client.Extensions;
using EntityG.Client.Models;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;

namespace EntityG.Client.Pages.Account.Settings
{
    public partial class BaseView
    {
        private CurrentUser _currentUser = new CurrentUser();

        private readonly UpdateProfileRequest _profileModel = new UpdateProfileRequest();

        private string UserId { get; set; }

        private char FirstLetterOfName { get; set; }


        private IBrowserFile file { get; set; }

        private string ImageDataUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadDataAsync();
        }


        private async Task UpdateProfileAsync()
        {
            var response = await _accountManager.UpdateProfileAsync(_profileModel);
            if (response.Succeeded)
            {
                await _authenticationManager.Logout();
                await _message.Success("Your Profile has been updated. Please Login to Continue.");
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        
        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            _profileModel.Email = user.GetEmail();
            _profileModel.FirstName = user.GetFirstName();
            _profileModel.LastName = user.GetLastName();
            _profileModel.PhoneNumber = user.GetPhoneNumber();
            UserId = user.GetUserId();
            var data = await _accountManager.GetProfilePictureAsync(UserId);
            if (data.Succeeded)
            {
                ImageDataUrl = data.Data;
            }
            if (_profileModel.FirstName.Length > 0)
            {
                FirstLetterOfName = _profileModel.FirstName[0];
            }
        }

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            file = e.File;
            if (file != null)
            {
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 250, 250);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                ImageDataUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                var request = new UpdateProfilePictureRequest() { ProfilePictureDataUrl = ImageDataUrl };
                var result = await _accountManager.UpdateProfilePictureAsync(request, UserId);
                if (result.Succeeded)
                {
                    _navigationManager.NavigateTo("/account", true);
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
}