using AntDesign;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Leaves;
using EntityG.Contracts.Responses.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.SelfServices.Leaves
{
    public partial class Leaves
    {
        private List<LeaveDto> _leaves = new List<LeaveDto>();
        private List<LookupDto> _employees = new List<LookupDto>();

        private int Page { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private int TotalCount { get; set; }
        private DateTime FromDate { get; set; } = DateTime.UtcNow.AddDays(-30);
        private DateTime ToDate { get; set; } = DateTime.UtcNow;
        private bool? IsApproved { get; set; } 
        private bool IsLoading { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllLeavesWithPaging();
        }
        private async Task GetAllLeavesWithPaging()
        {
            IsLoading = true;
            var response = await _leaveManager.GetAllAsync(Page, PageSize, FromDate, ToDate, IsApproved);
            if (response.Succeeded)
            {
                _leaves = response.Data;
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

            await GetAllEmployees();
        }

        private async Task HandlePageSizeChange(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetAllEmployees();
        }

        private void CreateLeaveRequest()
        {
            _navigationManager.NavigateTo("/self-service/leave/create");
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

        private void Edit(int id)
        {
            _navigationManager.NavigateTo($"/employee/{id}");
        }

        private async Task ShowDeleteConfirm(EmployeeDto employee)
        {
            var content = $"Are you sure to delete employee  '{employee.EmployeeIdNumber}' ?";
            var title = "Delete confirmation";
            var confirmResult = await _confirmService.Show(content, title, ConfirmButtons.YesNo);
            if (confirmResult == ConfirmResult.Yes)
            {

            }
        }
    }
}
