using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Services
{
    public interface IZoomService
    {
        public Task<MeetingLinks> getScheduledLinks(MeetingSettings meetingSettings);
    }
}
