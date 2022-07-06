using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Attendance_System.Models
{
    public class Statistics
    {
        public string ChartType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UId { get; set; }
    }
}
