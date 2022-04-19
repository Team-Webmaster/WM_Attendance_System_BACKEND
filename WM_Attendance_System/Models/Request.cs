#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Request
    {
        public int RequestId { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public float Duration { get; set; }
        public string SpecialNote { get; set; }
        public string Status { get; set; }
        public int SenderId { get; set; }
        public int? LeaveTypeId { get; set; }
        public int ApprovalId { get; set; }

    }
}