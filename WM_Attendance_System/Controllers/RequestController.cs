using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
        private readonly IMailService mailService;

        public RequestController(Hybrid_Attendance_SystemContext context, IMailService mailService)
        {
            _context = context;
            this.mailService = mailService;
        }

        // GET: api/Request
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PendingEmergencyLeave>>> GetPendingERequests()
        {
            return await _context.PendingEmergencyLeaves.ToListAsync();
        }

        // GET: api/Request/leave/5
        [HttpGet("leave/{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        //GET: api/Request/short-leave/5
        [HttpGet("short-leave/{id}")]
        public async Task<ActionResult<ShortLeaveRequest>> GetShortLeaveRequest(int id)
        {
            var request = await _context.ShortLeaves.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        //GET: api/Request/pending-leave-requests
        [HttpGet("pending-leave-requests")]
        public async Task<ActionResult> GetPendingRequests()
        {
            var pendingLeaves = await _context.PendingRequests.ToListAsync();
            var pendingShortLeaves = await _context.PendingShortLeaveRequests.ToListAsync();
            if (!pendingLeaves.Any() && !pendingShortLeaves.Any())
            {
                return Ok(new { state = false, message = "No pending requests available" });
            }
            return Ok(new { state = true, pendingLeaves=pendingLeaves, pendingShortLeaves=pendingShortLeaves });
        }

        private async Task<ActionResult> ApproveLeave(int requestId, int approvalId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if(request is null)
            {
                return BadRequest();
            }
            request.Status = "Approved";
            request.ApprovalId = approvalId;
            _context.Entry(request).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var requester = await _context.Users.FindAsync(request.SenderId);
                var approval = await _context.Users.FindAsync(request.ApprovalId);
                List<string> Emails = new List<string>();
                Emails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = Emails.ToArray(),
                    Subject = "Leave Request Granted.",
                    Body = $"Dear {requester.Name}, Your Leave request is Approved by {approval.Name}."
                };
                await mailService.SendEmailAsync(mailRequest);
                return Ok(new {  message = "Leave Request Approved Successfully Completed." });
            }
            catch(DbUpdateConcurrencyException)
            {
                if (RequestExists(requestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<ActionResult> ApproveShortLeave(int requestId, int approvalId)
        {
            var shortLeaveRequest = await _context.ShortLeaves.FindAsync(requestId);
            if(shortLeaveRequest is null)
            {
                return BadRequest();
            }
            shortLeaveRequest.Status = "Approved";
            shortLeaveRequest.ApprovalId = approvalId;
            _context.Entry(shortLeaveRequest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var requester = await _context.Users.FindAsync(shortLeaveRequest.SenderId);
                var approval = await _context.Users.FindAsync(shortLeaveRequest.ApprovalId);
                List<string> Emails = new List<string>();
                Emails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = Emails.ToArray(),
                    Subject = "Short Leave Request Granted.",
                    Body = $"Dear {requester.Name}, Your Short Leave request is Approved by {approval.Name}"
                };
                await mailService.SendEmailAsync(mailRequest);
                return Ok(new {  message = "Short Leave Request Approved Successfully Completed." });
            }
            catch(DbUpdateConcurrencyException)
            {
                if (ShortLeaveRequestExists(requestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<ActionResult> RejectLeaveRequest(int requestId, int approvalId)
        {
            var request = await _context.Requests.FindAsync(requestId);
            if(request is null)
            {
                return BadRequest();
            }
            request.Status = "Rejected";
            request.ApprovalId = approvalId;
            _context.Entry(request).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var requester = await _context.Users.FindAsync(request.SenderId);
                var approval = await _context.Users.FindAsync(request.ApprovalId);
                List<string> Emails = new List<string>();
                Emails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = Emails.ToArray(),
                    Subject = "Leave Request Rejected.",
                    Body = $"Dear {requester.Name}, Your Leave request is Rejected by {approval.Name}. Try for emergency leave from below link."
                };
                await mailService.SendEmailAsync(mailRequest);
                return Ok(new { message = "Leave request rejected successfully completed." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (RequestExists(requestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<ActionResult> RejectShortLeave(int requestId, int approvalId)
        {
            var shortLeaveRequest = await _context.ShortLeaves.FindAsync(requestId);
            if(shortLeaveRequest is null)
            {
                return BadRequest();
            }
            shortLeaveRequest.Status = "Rejected";
            shortLeaveRequest.ApprovalId = approvalId;
            _context.Entry(shortLeaveRequest).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                var requester = await _context.Users.FindAsync(shortLeaveRequest.SenderId);
                var approval = await _context.Users.FindAsync(shortLeaveRequest.ApprovalId);
                List<string> Emails = new List<string>();
                Emails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = Emails.ToArray(),
                    Subject = "Short Leave Request Rejected.",
                    Body = $"Dear {requester.Name}, Your Short Leave request is Rejected by {approval.Name}.If you want You can try it as emergency from below link."
                };
                await mailService.SendEmailAsync(mailRequest);
                return Ok(new { message = "Short Leave request rejected successfully completed." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ShortLeaveRequestExists(requestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // PUT: api/Request/leave-request/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("leave-request/{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        //PUT: api/Request/short-leave-request/5
        [HttpPut("short-leave-request/{id}")]
        public async Task<IActionResult> PutShortLeaveRequest(int id, ShortLeaveRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShortLeaveRequestExists(id))
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

        // POST: api/Request/leave-request
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("leave-request")]
        public async Task<ActionResult<Request>> PostRequest([FromForm]Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.RequestId }, request);
        }

        [HttpPost("emergency-leaves")]
        public async Task<ActionResult<EmergencyLeaveDetail>> PostELRequest([FromForm] EmergencyLeaveDetail emergencyLeave)
        {
            _context.EmergencyLeaveDetails.Add(emergencyLeave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostELRequest", new { id = emergencyLeave.Id }, emergencyLeave);
        }

        //POST: api/Request/short-leave-request
        [HttpPost("short-leave-request")]
        public async Task<ActionResult<ShortLeaveRequest>> PostShortLeaveRequest([FromForm] ShortLeaveRequest request)
        {
            _context.ShortLeaves.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShortLeaveRequest", new { id = request.Id }, request);
        }

        //POST: api/Request/approve-leave
        [HttpPost("approve-leave")]
        public async Task<ActionResult> ApproveLeaveRequests(RequestApproveDetails requestApproveDetails)
        {
            if (requestApproveDetails.Decision == "Approved")
            {
                return await ApproveLeave(requestApproveDetails.RequestId, requestApproveDetails.ApprovalId);
            }
            else if (requestApproveDetails.Decision == "Rejected")
            {
                return await RejectLeaveRequest(requestApproveDetails.RequestId, requestApproveDetails.ApprovalId);
            }
            return BadRequest();
        }
        
        //POST: api/Request/approve-short-leave
        [HttpPost("approve-short-leave")]
        public async Task<ActionResult> ApproveShortLeaveRequests(RequestApproveDetails requestApproveDetails)
        {
            if (requestApproveDetails.Decision == "Approved")
            {
                return await ApproveShortLeave(requestApproveDetails.RequestId, requestApproveDetails.ApprovalId);
            }
            else if (requestApproveDetails.Decision == "Rejected")
            {
                return await RejectShortLeave(requestApproveDetails.RequestId, requestApproveDetails.ApprovalId);
            }
            return BadRequest();
        }

        // DELETE: api/Request/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.RequestId == id);
        }

        private bool ShortLeaveRequestExists(int id)
        {
            return _context.ShortLeaves.Any(e => e.Id == id);
        }
    }
}
