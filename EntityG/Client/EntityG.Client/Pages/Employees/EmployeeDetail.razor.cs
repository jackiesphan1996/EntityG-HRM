using AntDesign;
using EntityG.Contracts.Requests.Employees;
using EntityG.Contracts.Responses.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace EntityG.Client.Pages.Employees
{
    public partial class EmployeeDetail
    {
        [Parameter]
        public string Id { get; set; }

        private List<LookupDto> _allEmployees = new List<LookupDto>();

        private List<LookupDto> _allSystemUsers = new List<LookupDto>();

        private List<LookupDto> _filteredSystemUsers = new List<LookupDto>();

        private List<LookupDto> _allDepartments = new List<LookupDto>();

        private UpdateEmployeeDto _employeeModel = new UpdateEmployeeDto();

        private Form<UpdateEmployeeDto> EmployeeDialogForm { get; set; }

        private string EmployeeDialogTitle { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var allTasks = new List<Task>
            {
                InitialForm(),
                GetAllEmployees(),
                GetAllDepartments(),
                GetAllSystemUsers()
            };

            await Task.WhenAll(allTasks);

            if (!string.IsNullOrEmpty(_employeeModel.SystemUserId))
            {
                _filteredSystemUsers = new List<LookupDto>
                {
                  _allSystemUsers.First(x => x.Id == _employeeModel.SystemUserId)
                };
            }
            
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
                var response = await _employeeManager.UpdateAsync(_employeeModel);
                if (response.Succeeded)
                {
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

        private async Task InitialForm()
        {
            var isValid = Int32.TryParse(Id, out var id);

            if (!isValid)
            {
                _navigationManager.NavigateTo("/exception/404");
            }

            var response = await _employeeManager.GetByIdAsync(id);
            if (response.Succeeded)
            {
                var item = response.Data;
                _employeeModel = new UpdateEmployeeDto
                {
                    Id = item.Id,
                    EmployeeIdNumber = item.EmployeeIdNumber,
                    Phone = item.Phone,
                    AccountNumber = item.AccountNumber,
                    Email = item.Email,
                    AccountTitle = item.AccountTitle,
                    Address1 = item.Address1,
                    Address2 = item.Address2,
                    BankName = item.BankName,
                    BasicSalary = item.BasicSalary,
                    City = item.City,
                    Country = item.Country,
                    DateOfBirth = item.DateOfBirth,
                    DepartmentId = item.DepartmentId,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Gender = item.Gender,
                    JoiningDate = item.JoiningDate,
                    LeavingDate = item.LeavingDate,
                    MaritalStatus = item.MaritalStatus,
                    PlaceOfBirth = item.PlaceOfBirth,
                    StateProvince = item.StateProvince,
                    UnpaidLeavePerDay = item.UnpaidLeavePerDay,
                    SupervisorId = item.SupervisorId,
                    SystemUserId = item.SystemUserId,
                    ZipCode = item.ZipCode
                };
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
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