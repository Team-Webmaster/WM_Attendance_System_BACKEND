#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WM_Attendance_System.Models
{
    public partial class MultipleID
    {
        [NotMapped]
        public int[] Ids { get; set; }
    }
}