#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Setting
    {
        public int SettingsId { get; set; }
        public int? NoOfWorkingHoursPerDay { get; set; }
        public int? NoOfAnnualLeaves { get; set; }
        public int? NoOfWorkingDaysPerWeek { get; set; }
        public int? AdminId { get; set; }

        public virtual User Admin { get; set; }
    }
}