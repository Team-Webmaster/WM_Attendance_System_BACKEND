#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class LeaveCalendarEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Comment { get; set; }
        public int? LeaveId { get; set; }
        public int? UserId { get; set; }

        public virtual LeaveDetail Leave { get; set; }
        public virtual UserTable User { get; set; }
    }
}