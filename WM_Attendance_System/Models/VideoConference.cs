#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WM_Attendance_System.Models
{
    public partial class VideoConference
    {
        public string ConferenceId { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int HostId { get; set; }
        public int SchedulerId { get; set; }
        [NotMapped]
        public int[] Participants { get; set; }
    }
}