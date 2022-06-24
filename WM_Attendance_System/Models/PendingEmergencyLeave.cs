using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class PendingEmergencyLeave
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public string Nic { get; set; }
        public string ProfilePic { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
