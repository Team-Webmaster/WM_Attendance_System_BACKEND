#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class PendingRequestView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string Nic { get; set; }
        public string DurationType { get; set; }
        public string SpecialNote { get; set; }
        public float Duration { get; set; }
        public string ProfilePic { get; set; }
    }
}