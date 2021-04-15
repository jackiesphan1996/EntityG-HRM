using AntDesign;
using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Shared;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.Employees
{
    public partial class CreateEmployee
    {
        private List<LookupDto> _allEmployees = new List<LookupDto>();

        private List<LookupDto> _allSystemUsers = new List<LookupDto>();

        private List<LookupDto> _filteredSystemUsers = new List<LookupDto>();

        private List<LookupDto> _allDepartments = new List<LookupDto>();

        private CreateEmployeeDto _employeeModel = new CreateEmployeeDto();

        private Form<CreateEmployeeDto> EmployeeDialogForm { get; set; }

        private string employeeEmail;

        private string EmployeeDialogTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {

            InitialForm();

            var allTasks = new List<Task>
            {
                GetAllEmployees(),
                GetAllDepartments(),
                GetAllSystemUsers()
            };

            await Task.WhenAll(allTasks);
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

        private async Task GetAllSystemUsers()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _allSystemUsers = response.Data;
                _filteredSystemUsers = _allSystemUsers.Take(10).ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task GetAllDepartments()
        {
            var response = await _departmentManager.GetAllAsync();
            if (response.Succeeded)
            {
                _allDepartments = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task HandleSubmit()
        {
            if (this.EmployeeDialogForm.Validate())
            {
                var response = await _employeeManager.CreateAsync(_employeeModel);
                if (response.Succeeded)
                {
                    _navigationManager.NavigateTo($"/employees");
                    await _message.Success(response.Messages[0]);
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

        private void InitialForm()
        {
            _employeeModel = new CreateEmployeeDto
            {
                DateOfBirth = DateTime.Now,
                JoiningDate = DateTime.Now
            };
        }

        private void Close()
        {
            _navigationManager.NavigateTo("/employees");
        }

        void OnSelectionChange(string searchValue)
        {
           _filteredSystemUsers = _allSystemUsers.Where(x => x.Value.Contains(searchValue)).Take(10).ToList();
            StateHasChanged();
        }

    }
}