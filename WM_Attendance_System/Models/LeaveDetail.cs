#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class LeaveDetail
    {
        public LeaveDetail()
        {
            LeaveCalendarEvents = new HashSet<LeaveCalendarEvent>();
        }

        public int LeaveId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public int? ApprovalId { get; set; }
        public int? LeaveTypeId { get; set; }

        public virtual UserTable Approval { get; set; }
        public virtual Leave LeaveType { get; set; }
        public virtual ICollection<LeaveCalendarEvent> LeaveCalendarEvents { get; set; }
    }
}