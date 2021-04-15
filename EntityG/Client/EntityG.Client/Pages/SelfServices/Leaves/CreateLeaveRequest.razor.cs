using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.SelfServices.Leaves
{
    public partial class CreateLeaveRequest
    {
        private List<LookupDto> _leaveTypes = new List<LookupDto>();
        private List<LookupDto> _employees = new List<LookupDto>();
        private readonly BasicFormModel _model = new BasicFormModel();

        private string radioValue4 = "";


        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            await GetAllLeaveTypes();
            await GetAllEmployees();
        }

        private void HandleSubmit()
        {
        }

        private async Task GetAllLeaveTypes()
        {
            var response = await _leaveTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _leaveTypes = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task GetAllEmployees()
        {
            var response = await _employeeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _employees = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }
    }
}
