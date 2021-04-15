using AntDesign;
using EntityG.Contracts.Requests.Department;
using EntityG.Contracts.Responses.Department;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.Departments
{
    public partial class Departments
    {
        private List<DepartmentDto> _departments = new List<DepartmentDto>();

        private DepartmentDto _department = new DepartmentDto();

        private int Page { get; set; } = 1;

        private int PageSize { get; set; } = 10;

        private int TotalCount { get; set; }

        private string SearchText { get; set; } = "";

        private bool IsLoading { get; set; }

        private bool DepartmentDialogVisible { get; set; }

        private string DepartmentDialogTitle { get; set; }

        private Form<DepartmentDto> DepartmentDialogForm { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAllDepartmentsWithPaging();
        }

        private async Task GetAllDepartmentsWithPaging()
        {
            IsLoading = true;
            var response = await _departmentManager.GetAllAsync(Page, PageSize, SearchText);
            if (response.Succeeded)
            {
                _departments = response.Data;
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

            await GetAllDepartmentsWithPaging();
        }

        private async Task HandlePageSizeChange(PaginationEventArgs args)
        {
            Page = args.PageIndex;
            PageSize = args.PageSize;

            await GetAllDepartmentsWithPaging();
        }

        private void CreateDepartment()
        {
            _department = new DepartmentDto();
            DepartmentDialogVisible = true;
            DepartmentDialogTitle = "Create department";
        }

        private void HandleDepartmentDialogCancel()
        {
            DepartmentDialogVisible = false;
        }

        private async Task HandleDepartmentDialogOk()
        {
            if (this.DepartmentDialogForm.Validate())
            {
                await SaveAsync(_department);
            }
        }

        private void Edit(int id)
        {
            var existedDepartment = _departments.FirstOrDefault(x => x.Id.Equals(id));
            _department = new DepartmentDto
            {
                Id = id,
                Name = existedDepartment.Name,
                Description = existedDepartment.Description
            };
            DepartmentDialogVisible = true;
            DepartmentDialogTitle = "Update department";
        }

        private async Task ShowDeleteConfirm(DepartmentDto department)
        {
            var content = $"Are you sure to delete Department  '{department.Name}' ?";
            var title = "Delete confirmation";
            var confirmResult = await _confirmService.Show(content, title, ConfirmButtons.YesNo);
            if (confirmResult == ConfirmResult.Yes)
            {
                await Delete(department);
            }
        }

        private async Task SaveAsync(DepartmentDto department)
        {
            if (department.Id == 0)
            {
                await CreateAsync(department);
            }
            else
            {
                await UpdateAsync(department);
            }
        }

        private async Task CreateAsync(DepartmentDto department)
        {
            var request = new CreateDepartmentDto { Name = department.Name, Description = department.Description };

            var response = await _departmentManager.CreateAsync(request);

            if (response.Succeeded)
            {
                await GetAllDepartmentsWithPaging();
                DepartmentDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task UpdateAsync(DepartmentDto department)
        {
            var request = new UpdateDepartmentDto()
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };

            var response = await _departmentManager.UpdateAsync(request);

            if (response.Succeeded)
            {
                await GetAllDepartmentsWithPaging();
                DepartmentDialogVisible = false;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task Delete(DepartmentDto department)
        {
            var response = await _departmentManager.DeleteAsync(department.Id);
            if (response.Succeeded)
            {
                await GetAllDepartmentsWithPaging();
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