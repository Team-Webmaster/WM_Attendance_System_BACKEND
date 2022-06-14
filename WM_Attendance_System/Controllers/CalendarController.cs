using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Data;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public CalendarController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetEvents(int userId)
        {
            var calendarEvents = await _context.CalendarEventsByUser(userId).ToListAsync();
            var shortCalendarEvents = await _context.ShortCalendarEventsByUser(userId).ToListAsync();
            return Ok(new { calendarEvents=calendarEvents, shortCalendarEvents=shortCalendarEvents });
        }

    }
}
