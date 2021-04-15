using System;
using System.ComponentModel.DataAnnotations;

namespace EntityG.EntityFramework.Entities
{
    public class Leave : AuditableEntity
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string EmergencyContact { get; set; }

        public bool IsApproved { get; set; }

        public string ApproveBy { get; set; }

        public LeaveType LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public Employee Employee { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public bool IsPaidLeave { get; set; }
    }
}
