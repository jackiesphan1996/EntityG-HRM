using AntDesign;
using EntityG.Client.Infrastructure.ViewModels;
using EntityG.Contracts.Responses.Employees;
using EntityG.Contracts.Responses.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.Employees
{
    public partial class Employees
    {
        private List<EmployeeDto> _employees = new List<EmployeeDto>();

        private List<LookupDto> _allEmployees = new List<LookupDto>();

        private EmployeeRequest _employeeModel = new EmployeeRequest();

        private string searchString = "";
        private int Page { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private int TotalCount { get; set; }
        private string SearchText { get; set; } = "";
        private bool IsLoading { get; set; }
        private bool EmployeeDialogVisible { get; set; }
        
        private string EmployeeDialogTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllEmployeesWithPaging();
        }
        private async Task GetAllEmployeesWithPaging()
        {
            IsLoading = true;
            var response = await _employeeManager.GetAllAsync(Page, PageSize, SearchText);
            if (response.Succeeded)
            {
                _employees = response.Data;
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

        private void CreateEmployee()
        {
            _navigationManager.NavigateTo("/employee/create");
        }

        private void HandleEmployeeDialogCancel()
        {
            EmployeeDialogVisible = false;
        }


        private async Task GetAllEmployees()
        {
            var response = await _employeeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _allEmployees = response.Data;
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