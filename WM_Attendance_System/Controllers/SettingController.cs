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
    public class SettingController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public SettingController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<Setting>> GetSettings()
        {
	    int id = 1;
            return await _context.Settings.FindAsync(id);
        }

        // DELETE: api/Settings
        [HttpPut]
        public async Task<IActionResult> UpdateSettings(Setting setting)
        {

            _context.Entry(setting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(1))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.SettingsId == id);
        }

    }
}
