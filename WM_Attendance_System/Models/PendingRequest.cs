#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class PendingRequest
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Nic { get; set; }
        public string Type { get; set; }
    }
}