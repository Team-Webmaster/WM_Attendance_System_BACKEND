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
    public class LeaveDetailController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public LeaveDetailController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        [HttpGet("userId")]
        public async Task<ActionResult<IEnumerable<LeaveDetail>>> GetLeaveDetails(int userId)
        {
            return await _context.LeaveDetails.Where(leaveDetail=>leaveDetail.SenderId==userId).ToListAsync();
        }

        [HttpGet("leave-types")]
        public async Task<ActionResult<IEnumerable<Leave>>> GetLeaveTypes()
        {
            return await _context.Leaves.ToListAsync();
        }
    }
}
