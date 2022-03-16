using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackListedEmailController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public BlackListedEmailController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        // GET: api/BlackListedEmails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlackListedEmail>>> GetBlackListedEmails()
        {
            return await _context.BlackListedEmails.ToListAsync();
        }

        // DELETE: api/BlackListedEmails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlackListedEmail(string id)
        {
            var blackListedEmail = await _context.BlackListedEmails.FindAsync(id);
            if (blackListedEmail == null)
            {
                return NotFound();
            }

            _context.BlackListedEmails.Remove(blackListedEmail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
