using AntDesign;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;
using EntityG.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfirmButtons = AntDesign.ConfirmButtons;
using ConfirmResult = AntDesign.ConfirmResult;

namespace EntityG.Client.Pages.Roles
{
    public partial class Roles
    {
        public List<RoleResponse> RoleList = new List<RoleResponse>();

        private RoleResponse _role = new RoleResponse();

        private string searchString = "";

        private static string CREATED_ROLE  = "Created Role";

        private bool RoleDiaglogVisiable { get; set; }

        private Form<RoleResponse> RoleDiaglogForm { get; set; }

        private string RoleDiaglogTitle { get; set; }


        private bool IsLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetRolesAsync();
        }

        private async Task GetRolesAsync()
        {
            IsLoading = true;

            var response = await _roleManager.GetRolesAsync();
            if (response.Succeeded)
            {
                RoleList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    
                }
            }

            IsLoading = false;
        }

        private void Create()
        {
            _role = new RoleResponse();
            RoleDiaglogTitle = "Create role";
            RoleDiaglogVisiable = true;
        }

        private void Edit(string id)
        {
            _role = new RoleResponse
            {
                Id = id,
                Name = RoleList.FirstOrDefault(x => x.Id.Equals(id))?.Name
            };

            RoleDiaglogTitle = "Update role";
            RoleDiaglogVisiable = true;
        }


        private async Task SaveAsync(RoleResponse item)
        {
            var roleRequest = new RoleRequest() { Name = item.Name, Id = item.Id };
            var result = await _roleManager.SaveAsync(roleRequest);

            if (result.Succeeded)
            {
                RoleDiaglogVisiable = false;
                await GetRolesAsync();
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    await _message.Error(message);
                }
            }

        }

        private async Task DeleteRole(RoleResponse role)
        {
            var response = await _roleManager.DeleteAsync(role.Id);
            if (response.Succeeded)
            {
                RoleList.Remove(role);
                await GetRolesAsync();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task ShowDeleteConfirm(RoleResponse role)
        {
            var content = $"Are you sure to delete role '{role.Name}' ?";
            var title = "Delete confirmation";
            var confirmResult = await _confirmService.Show(content, title, ConfirmButtons.YesNo);
            if (confirmResult == ConfirmResult.Yes)
            {
                await DeleteRole(role);
            }
        }

        private void ManagePermissions(string roleId)
        {
            _navigationManager.NavigateTo($"/identity/role-permissions/{roleId}");
        }

        private async Task HandleRoleDiaglogOk()
        {
            if (this.RoleDiaglogForm.Validate())
            {
                await SaveAsync(_role);
            }
            
        }

        private void HandleRoleDiaglogCancel()
        {
            RoleDiaglogVisiable = false;
        }
    }
}