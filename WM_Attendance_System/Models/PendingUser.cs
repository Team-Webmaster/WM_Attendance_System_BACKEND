#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class PendingUser
    {
        public int PendingUserId { get; set; }
        public string Name { get; set; }
        public string Nic { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string ProfilePic { get; set; }
        public int? Type { get; set; }
        public int? NoOfAnnualLeaves { get; set; }
        public string Status { get; set; }
    }
}