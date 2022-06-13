using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var attendHours = _context.CalendarEventsByUser(1);

            return Ok(new { data=attendHours });
        }

    }
}
