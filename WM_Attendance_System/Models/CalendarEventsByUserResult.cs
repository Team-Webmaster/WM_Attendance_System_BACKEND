using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WM_Attendance_System.Models
{
    public partial class CalendarEventsByUserResult
    {
        public int? userId { get; set; }
        public string userName { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        public string comment { get; set; }
    }
}
