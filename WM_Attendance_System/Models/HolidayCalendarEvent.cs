#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class HolidayCalendarEvent
    {
        public HolidayCalendarEvent()
        {
            UIds = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Comment { get; set; }
        public int? EditorId { get; set; }

        public virtual User Editor { get; set; }

        public virtual ICollection<User> UIds { get; set; }
    }
}