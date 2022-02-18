#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class HolidayCalendarEvent1
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Comment { get; set; }
    }
}