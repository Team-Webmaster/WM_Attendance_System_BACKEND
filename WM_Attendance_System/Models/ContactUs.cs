#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WM_Attendance_System.Models
{
    public partial class ContactUs
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Message { get; set; }
	public int UId { get; set; }
    }
}