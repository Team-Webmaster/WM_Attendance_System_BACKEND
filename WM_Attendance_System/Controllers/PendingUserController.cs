using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PendingUserController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public PendingUserController(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }


        // GET: api/PendingUser
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingUser>>> GetPendingUsers()
        {
            return await _context.PendingUsers.ToListAsync();
        }

        // GET: api/PendingUser/approve/5
        [HttpGet("approve/{id}")]
        public async Task<ActionResult<User>> ApprovePendingUser(int id)
        {
            var pendingUser = await _context.PendingUsers.FindAsync(id);

            if (pendingUser == null)
            {
                return NotFound(); 
            }
            pendingUser.Status = "Approved";
            _context.Entry(pendingUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { state = true, message = "New user approved succesfully completed." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/PendingUser/ApproveAll
        [HttpPost("ApproveAll")]
        public async Task<ActionResult<User>> ApproveAllPendingUser(MultipleID multipleID)
        {
            foreach (var id in multipleID.Ids)
            {
                var pendingUser = await _context.PendingUsers.FindAsync(id);

                if (pendingUser == null)
                {
                    return NotFound();
                }
                pendingUser.Status = "Approved";
                _context.Entry(pendingUser).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PendingUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok();
        }

        // GET: api/PendingUser/reject/5
        [HttpGet("reject/{id}")]
        public async Task<ActionResult<User>> RejectPendingUser(int id)
        {
            var pendingUser = await _context.PendingUsers.FindAsync(id);

            if (pendingUser == null)
            {
                return NotFound();
            }
            pendingUser.Status = "Rejected";
            _context.Entry(pendingUser).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { state = true, message = "New user rejected succesfully completed." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PendingUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/PendingUser/RejectAll
        [HttpPost("RejectAll")]
        public async Task<ActionResult<User>> RejectAllPendingUser(MultipleID multipleID)
        {
            foreach (var id in multipleID.Ids)
            {
                var pendingUser = await _context.PendingUsers.FindAsync(id);

                if (pendingUser == null)
                {
                    return NotFound();
                }
                pendingUser.Status = "Rejected";
                _context.Entry(pendingUser).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PendingUserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok();
        }

        private bool PendingUserExists(int id)
        {
            return _context.PendingUsers.Any(e => e.PendingUserId == id);
        }
    }
}
