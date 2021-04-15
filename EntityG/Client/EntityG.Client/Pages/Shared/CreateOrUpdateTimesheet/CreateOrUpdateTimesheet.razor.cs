using AntDesign;
using EntityG.Contracts.Requests.Timesheets;
using EntityG.Contracts.Responses.Shared;
using EntityG.Client.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntityG.Client.Pages.Shared.CreateOrUpdateTimesheet
{
    public partial class CreateOrUpdateTimesheet
    {
        [Parameter] public TimesheetRequest Timesheet { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public bool Visiable { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> HandleOkay { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> HandleCancel { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> HandleRemove { get; set; }

        private List<LookupDto> _projects = new List<LookupDto>();

        private string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetAlProjects();
        }

        private async Task GetAlProjects()
        {
            var response = await _projectManager.GetAllAsync();
            if (response.Succeeded)
            {
                _projects = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    await _message.Error(message);
                }
            }
        }

        private async Task Create(TimesheetRequest timesheetVm)
        {
            var timesheet = new CreateTimesheetRequest
            {
                HourRate = timesheetVm.HourRate,
                ProjectId = timesheetVm.ProjectId,
                Activity = timesheetVm.Activity,
                Hours = timesheetVm.Hours,
                Comment = timesheetVm.Comment,
                Date = timesheetVm.Date == default ? DateTime.Now.Date : timesheetVm.Date
            };

            await _timesheetManager.CreateAsync(timesheet);
        }

        private async Task Update(TimesheetRequest timesheetVm)
        {
            var timesheet = new UpdateTimesheetRequest
            {
                Id = timesheetVm.Id,
                HourRate = timesheetVm.HourRate,
                ProjectId = timesheetVm.ProjectId,
                Activity = timesheetVm.Activity,
                Hours = timesheetVm.Hours,
                Comment = timesheetVm.Comment,
                Date = timesheetVm.Date == default ? DateTime.Now.Date : timesheetVm.Date
            };

            await _timesheetManager.UpdateAsync(timesheet);
        }

        private async Task HandleOk(MouseEventArgs e)
        {
            try
            {
                if (Timesheet.Id == 0)
                {
                    await Create(Timesheet);
                    Visiable = false;
                    await HandleOkay.InvokeAsync(e);
                    await NotifyCreateSuccess();
                }
                else
                {
                    await Update(Timesheet);
                    await HandleOkay.InvokeAsync(e);
                    await NotifyUpdateSuccess();
                }
            }
            catch (System.Exception ex)
            {
                await NotifyError(ex.Message);
            }
        }

        private async Task OnRemove(int id)
        {
            var result = await  _timesheetManager.DeleteAsync(id);
            if (result.Succeeded)
            {
                Visiable = false;
                await HandleRemove.InvokeAsync();
                await NotifyDeleteSuccess();
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    await NotifyError(error);
                }
            }
        }

        private async Task HandleCloseDiaglog(MouseEventArgs e)
        {
            Visiable = false;
            await HandleCancel.InvokeAsync(e);
        }

        private async Task NotifyDeleteSuccess()
        {
            await _message.Success("Deleted successfully.");
        }

        private async Task NotifyUpdateSuccess()
        {
            await _message.Success("Updated successfully.");
        }

        private async Task NotifyCreateSuccess()
        {
            await _message.Success("Created successfully.");
        }

        private async Task NotifyError(string error)
        {
            await _message.Error(error);
        }
    }
}
