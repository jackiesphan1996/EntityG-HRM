using System;

namespace EntityG.Contracts.Responses.Leaves
{
    public class LeaveDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeIdNumber { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LeaveType { get; set; }
        public int LeaveTypeId { get; set; }
        public string Reason { get; set; }
        public bool IsApproved { get; set; }
        public string EmergencyContact { get; set; }
        public bool IsPaidLeave { get; set; }
    }
}
