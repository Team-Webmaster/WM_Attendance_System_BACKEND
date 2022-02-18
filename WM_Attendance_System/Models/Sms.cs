#nullable disable
using System;
using System.Collections.Generic;

namespace WM_Attendance_System.Models
{
    public partial class Sms
    {
        public Sms()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Messsage { get; set; }
        public int? AdminId { get; set; }

        public virtual User Admin { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}