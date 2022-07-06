#nullable disable
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WM_Attendance_System.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Type { get; set; }
        public int? UId { get; set; }
        public virtual User UIdNavigation { get; set; }
        [NotMapped]
        public IFormFile FaceImage { get; set; }
    }
}