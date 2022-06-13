#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; }
        public int? UId { get; set; }
        public int RequesterId { get; set; }
        public virtual User UIdNavigation { get; set; }
    }
}