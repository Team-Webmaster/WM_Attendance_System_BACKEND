using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class EmergencyLeaveDetail
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int RequesterId { get; set; }
        public int ApprovalId { get; set; }
        public string Status { get; set; }
    }
}
