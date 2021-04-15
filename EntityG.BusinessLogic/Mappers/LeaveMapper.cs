using EntityG.Contracts.Responses.Leaves;
using EntityG.EntityFramework.Entities;

namespace EntityG.BusinessLogic.Mappers
{
    public static class LeaveMapper
    {
        public static LeaveDto Map(Leave leave)
        {
            return new LeaveDto
            {
                Id = leave.Id,
                FromDate = leave.FromDate,
                ToDate = leave.ToDate,
                EmployeeName = leave.Employee.FullName,
                EmployeeIdNumber = leave.Employee.EmployeeIdNumber,
                ApprovedBy = leave.ApproveBy,
                IsApproved = leave.IsApproved,
                LeaveType = leave.LeaveType.Name,
                LeaveTypeId = leave.LeaveTypeId,
                EmergencyContact = leave.EmergencyContact,
                IsPaidLeave = leave.IsPaidLeave,
                Reason = leave.Description
            };
        }
    }
}
