using AntDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Identity;
using Microsoft.AspNetCore.Components.Web;

namespace EntityG.Client.Pages.Users
{
    public partial class Users
    {
        public List<UserResponse> UserList = new List<UserResponse>();

        private UserResponse user = new UserResponse();

        private string searchString = "";
        private int Page { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private int TotalCount { get; set; }
        private string SearchText { get; set; } = "";

        private bool IsLoading { get; set; }

        bool visible = false;

        void open()
        {
            this.visible = true;
        }

        void close()
        {
            this.visible = false;
        }

        private async Task HandleReload(MouseEventArgs e)
        {
            close();
        
            await GetUsersAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUsersAsync();
        }
        private async Task GetUsersAsync()
        {
            IsLoading = true;
            var response = await _userManager.GetAllAsync(Page, PageSize, SearchText);
            if (response.Succeeded)
            {
                UserList = response.Data.ToList();
                TotalCount = response.TotalCount;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    // add Message later
                }
            }

            IsLoading = false;
        }

        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (user.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        private void ViewProfile(string userId)
        {
            _navigationManager.NavigateTo($"/user-profile/{userId}");
        }

        private void ManageRoles(string userId, string email)
        {
            //if (email == "mukesh@blazorhero.com") _snackBar.Add("Not Allowed.", Severity.Error);
            //else _navigationManager.NavigateTo($"/identity/user-roles/{userId}");
            _navigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }

        private async Task HandlePageIndexChanged(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetUsersAsync();
        }

        private async Task HandlePageSizeChange(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetUsersAsync();
        }

        private void CreateUser()
        {
            _navigationManager.NavigateTo($"/identity/users/create");
        }
    }
}