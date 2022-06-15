using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class ShortCalendarEventsByUser
    {
        public string eventName { get; set; }
        public DateTime? date { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string comment { get; set; }
    }
}
