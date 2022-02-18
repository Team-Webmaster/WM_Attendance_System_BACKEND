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
    public class UserController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public UserController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        // GET: api/UserTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUserTables()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/UserTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserTable(int id)
        {
            var userTable = await _context.Users.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/UserTables/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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

        [HttpPost("login")]
        public async Task<ActionResult<User>> PostUserTable(Login login)
        {
            var User = await _context.Users.SingleOrDefaultAsync(x=> x.Email == login.Email);
            if (User is null)
            {
                var PendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == login.Email);
                if (PendingUser is null)
                {
                    return BadRequest(new { state = false, message = "Email not found" });
                }
                return Ok(new { state = false, message = "Your account is in approving process. Please Try again later." });
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
            var userTable = await _context.Users.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
