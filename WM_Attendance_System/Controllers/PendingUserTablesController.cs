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
    public class PendingUserTablesController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public PendingUserTablesController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        // GET: api/PendingUserTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingUserTable>>> GetPendingUserTables()
        {
            return await _context.PendingUserTables.ToListAsync();
        }

        // GET: api/PendingUserTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PendingUserTable>> GetPendingUserTable(int id)
        {
            var pendingUserTable = await _context.PendingUserTables.FindAsync(id);

            if (pendingUserTable == null)
            {
                return NotFound();
            }

            return pendingUserTable;
        }

        // PUT: api/PendingUserTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPendingUserTable(int id, PendingUserTable pendingUserTable)
        {
            if (id != pendingUserTable.PendingUserId)
            {
                return BadRequest();
            }
            pendingUserTable.Password = BCrypt.Net.BCrypt.HashPassword(pendingUserTable.Password);
            _context.Entry(pendingUserTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingUserTableExists(id))
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

        // POST: api/PendingUserTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PendingUserTable>> PostPendingUserTable(PendingUserTable pendingUserTable)
        {
            pendingUserTable.Password = BCrypt.Net.BCrypt.HashPassword(pendingUserTable.Password);
            _context.PendingUserTables.Add(pendingUserTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPendingUserTable", new { id = pendingUserTable.PendingUserId }, pendingUserTable);
        }

        // DELETE: api/PendingUserTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePendingUserTable(int id)
        {
            var pendingUserTable = await _context.PendingUserTables.FindAsync(id);
            if (pendingUserTable == null)
            {
                return NotFound();
            }

            _context.PendingUserTables.Remove(pendingUserTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PendingUserTableExists(int id)
        {
            return _context.PendingUserTables.Any(e => e.PendingUserId == id);
        }
    }
}
