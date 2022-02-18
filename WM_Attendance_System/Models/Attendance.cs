#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public string Type { get; set; }
        public int? UId { get; set; }

        public virtual User UIdNavigation { get; set; }
    }
}