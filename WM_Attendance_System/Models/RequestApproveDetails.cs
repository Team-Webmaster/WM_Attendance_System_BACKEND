using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class RequestApproveDetails
    {
        public int RequestId { get; set; }
        public int ApprovalId { get; set; }
        public string Decision { get; set; }
    }
}
