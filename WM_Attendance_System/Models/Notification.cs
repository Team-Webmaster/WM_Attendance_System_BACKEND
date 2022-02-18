#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public string Message { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }

        public virtual UserTable Receiver { get; set; }
        public virtual UserTable Sender { get; set; }
    }
}