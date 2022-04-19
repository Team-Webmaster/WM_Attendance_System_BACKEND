using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public partial class ShortLeaveRequest
    {
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string SpecialNote { get; set; }
		public int RequesterId { get; set; }
		public int ApprovalId { get; set; }
		public string Status { get; set; }
    }
}
