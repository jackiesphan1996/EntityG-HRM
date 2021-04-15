using AutoMapper;
using EntityG.Client.Infrastructure.Mappings;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Responses.Identity;

namespace EntityG.Client.Pages.RolePermissions
{
    public partial class RolePermissions
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        private PermissionResponse Model { get; set; } = new PermissionResponse
        {
            RoleClaims = new List<RoleClaimsResponse>()
        };

        [Parameter]
        public string Description { get; set; }

        private IMapper _mapper;


        protected override async Task OnInitializedAsync()
        {
            _mapper = new MapperConfiguration(c => { c.AddProfile<RoleProfile>(); }).CreateMapper();
            var roleId = Id;
            var result = await _roleManager.GetPermissionsAsync(roleId);
            if (result.Succeeded)
            {
                Model = result.Data;
                if (Model != null)
                {
                    Description = $"Manage {Model.RoleId} {Model.RoleName}'s Permissions";
                }
            }
        }
        private async Task SaveAsync()
        {
            var request = _mapper.Map<PermissionResponse, PermissionRequest>(Model);
            var result = await _roleManager.UpdatePermissionsAsync(request);
            if (result.Succeeded)
            {
                await _message.Success(result.Messages[0]);
                _navigationManager.NavigateTo("/identity/roles");
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