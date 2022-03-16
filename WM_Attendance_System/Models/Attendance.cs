﻿#nullable disable
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
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string Type { get; set; }
        public int UId { get; set; }
        [NotMapped]
        public IFormFile FaceImage { get; set; }
        public virtual User UIdNavigation { get; set; }
    }
}