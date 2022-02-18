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
    public class UserTablesController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public UserTablesController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        // GET: api/UserTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/UserTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTable>> GetUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/UserTables/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UserTable userTable)
        {
            if (id != userTable.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        // POST: api/UserTables
        //[HttpPost]
        //public async Task<ActionResult<UserTable>> PostUserTable(UserTable userTable)
        //{
        //    _context.UserTables.Add(userTable);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUserTable", new { id = userTable.UserId }, userTable);
        //}

        [HttpPost("login")]
        public async Task<ActionResult<UserTable>> PostUserTable(Login login)
        {
            var User = await _context.PendingUserTables.SingleOrDefaultAsync(x=> x.Email == login.Email);
            if (User is null)
            {
                return BadRequest(new { state = false, message = "Email not found" });
            }
            if (BCrypt.Net.BCrypt.Verify(login.Password, User.Password))
            {
                return Ok(new { state = true, data = User });
            }
            return BadRequest(new { state = false, message = "Password Incorrect" });

        }



        // DELETE: api/UserTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return _context.UserTables.Any(e => e.UserId == id);
        }
    }
}
