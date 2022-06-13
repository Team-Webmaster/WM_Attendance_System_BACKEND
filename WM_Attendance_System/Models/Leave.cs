#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Leave
    {
        public Leave()
        {
            LeaveDetails = new HashSet<LeaveDetail>();
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public int? AdminId { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<LeaveDetail> LeaveDetails { get; set; }
    }
}