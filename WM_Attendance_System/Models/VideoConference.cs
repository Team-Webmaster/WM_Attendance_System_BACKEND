#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class VideoConference
    {
        public VideoConference()
        {
            UIds = new HashSet<User>();
        }

        public int ConferenceId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string HostId { get; set; }
        public int? SchedulerId { get; set; }

        public virtual User Scheduler { get; set; }

        public virtual ICollection<User> UIds { get; set; }
    }
}