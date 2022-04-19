using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class PendingShortLeaveRequestsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Nic { get; set; }
        public string ProfilePic { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SpecialNote { get; set; }
    }
}
