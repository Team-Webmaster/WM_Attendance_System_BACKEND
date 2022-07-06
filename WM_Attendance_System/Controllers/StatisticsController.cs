using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public StatisticsController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> GetChartData(Statistics statistics)
        {
            int attendDates = await _context.Attendances
                .Where(a => a.Date <= statistics.EndDate && a.Date >= statistics.StartDate && a.Type == "Attend" && a.UId == statistics.UId)
                .CountAsync();
            int leaveRequests = await _context.LeaveDetails
                .Where(l => l.Date <= statistics.EndDate && l.Date >= statistics.StartDate && l.SenderId == statistics.UId)
                .CountAsync();
            float leaveDuration = await _context.LeaveDetails
                .Where(l => l.Date <= statistics.EndDate && l.Date >= statistics.StartDate && l.SenderId == statistics.UId)
                .SumAsync(l=>l.Duration);
            return Ok(new { attendDates = attendDates, leaveRequests = leaveRequests, leaveDuration = leaveDuration });
        }
    }
}
