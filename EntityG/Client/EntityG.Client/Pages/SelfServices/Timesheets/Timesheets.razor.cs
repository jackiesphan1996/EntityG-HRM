using AntDesign;
using EntityG.Client.Infrastructure.ViewModels;
using EntityG.EntityFramework.Entities;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityG.Contracts.Responses.Timesheets;

namespace EntityG.Client.Pages.SelfServices.Timesheets
{
    public partial class Timesheets
    {
        private DateTime filterDate = DateTime.UtcNow.ToLocalTime();

        private string PageTitle => $"EntityG - Dashboard";

        private List<TimesheetDto> AllData = new List<TimesheetDto>();

        private bool ShowDialog { get; set; }

        private string Title { get; set; }

        private TimesheetRequest Timesheet { get; set; }

        private bool IsAllowOpenPopup = false;

        private int TimeCount = 0;

        private bool isFirstTimeLoadPage = false;

        private async Task HandleReload(MouseEventArgs e)
        {
            ShowDialog = false;
            IsAllowOpenPopup = false;
            Action = SelfServices.Timesheets.Action.Add;
            await GetAllData();
        }

        private Action? Action;

        private bool IsClickEdit = true;

        private void HandleCloseDialog(MouseEventArgs e)
        {
            ShowDialog = false;
            Action = SelfServices.Timesheets.Action.Add;
        }


        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            isFirstTimeLoadPage = true;
            await GetAllData();
        }

        private void OnSelect(DateTime _value)
        {
            if (TimeCount == 0 || TimeCount == 1)
            {
                Action = SelfServices.Timesheets.Action.Add;
                TimeCount++;
            }
            else
            {
                TimeCount++;
                if (Action.HasValue)
                {
                    if (Action == SelfServices.Timesheets.Action.Add)
                    {
                        Action = SelfServices.Timesheets.Action.Add;
                        Title = "Create timesheet";
                        Timesheet = new TimesheetRequest
                        {
                            Date = _value.Date,
                            Activity = Activity.Code,
                            HourRate = HourRate.NormalWorkingDays,
                            Hours = 8
                        };

                        ShowDialog = true;
                    }
                }
            }

        }

        private void OnChange(DateTime value, string mode)
        {
            TimeCount = 0;
            Console.WriteLine($"Log : {value} - Mode : {mode}");
        }

        private async Task GetAllData()
        {

            await GetAllTimesheetAsync();
            if (isFirstTimeLoadPage)
            {
                IsAllowOpenPopup = false;
            }
        }

        private List<TimesheetDto> GetListData(DateTime value)
        {
            return AllData.Where(x => x.Date.Day == value.Day && x.Date.Month == value.Month && x.Date.Year == value.Year)
                          .ToList();
        }


        private void OnClick(int id)
        {
            Title = "Update timesheet";

            var currentTimesheet = AllData.FirstOrDefault(x => x.Id == id);

            Timesheet = new TimesheetRequest
            {
                Id = currentTimesheet.Id,
                ProjectId = currentTimesheet.ProjectId,
                HourRate = currentTimesheet.HourRate,
                Activity = currentTimesheet.Activity,
                Hours = currentTimesheet.Hours,
                Date = currentTimesheet.Date,
                Comment = currentTimesheet.Comment
            };

            IsClickEdit = true;

            Action = SelfServices.Timesheets.Action.Edit;

            ShowDialog = true;
        }

        private async Task OnRemove(int id)
        {
            ShowDialog = false;
            await _message.Success($"Deleted {id}");
        }

        private async Task OnDatePickerSelectedChange(DateTimeChangedEventArgs eventArgs)
        {
            filterDate = eventArgs.Date;
            await GetAllTimesheetAsync();
        }

        private async Task GetAllTimesheetAsync()
        {
            var startDate = new DateTime(filterDate.Year, filterDate.Month, 1).AddDays(-15);
            var endDate = new DateTime(filterDate.Year, filterDate.Month, DateTime.DaysInMonth(filterDate.Year, filterDate.Month)).AddDays(15);
            var result = await _timesheetManager.GetAllAsync(startDate, endDate);

            if (result.Succeeded)
            {
                AllData = result.Data;
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    await _message.Error(error);
                }
            }
        }

        private int? GetMonthData(DateTime value)
        {
            if (value.Month == 8)
            {
                return 1394;
            }

            return null;
        }
    }

    

    public enum Action
    {
        Edit,
        Add
    }
}