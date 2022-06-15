using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
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
    public class ReportController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
        private readonly IMailService mailService;

        public ReportController(Hybrid_Attendance_SystemContext context, IMailService mailService)
        {
            _context = context;
            this.mailService = mailService;
        }

        // GET: api/Report
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Report>>> GetReports()
        {
            return await _context.Reports.ToListAsync();
        }

        // GET: api/Report/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Report>> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        // PUT: api/Report/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReport(int id, Report report)
        {
            if (id != report.ReportId)
            {
                return BadRequest();
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
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

        // POST: api/Report
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", new { id = report.ReportId }, report);
        }

        // DELETE: api/Report/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("generate-report")]
        public async Task<ActionResult> GenerateReports(Report report)
        {
            if(report.Type=="Performance Report")
            {
                return await PerformanceReport(report);
            }
            else if(report.Type=="Attendee Report")
            {
                return await AttendeeReport(report);
            }
            else if(report.Type=="Leave Master")
            {
                return await LeaveMasterReport(report);
            }
            return BadRequest();
        }
        private async Task<ActionResult> PerformanceReport(Report report)
        {
            try
            {
                var requester = await _context.Users.FindAsync(report.RequesterId);
                List<string> toEmails = new List<string>();
                toEmails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = toEmails.ToArray(),
                    Subject = $"Report Created Successfully for UserID:{report.UId}",
                    Body = "You can download your requested Report."
                };
                var records = await _context.Attendances
                    .Where(attend => attend.Date >= report.StartDate && attend.Date <= report.EndDate && attend.UId == report.UId)
                    .Select(a=>new { a.Id, a.Date, a.Type, a.InTime, a.OutTime, a.UId })
                    .ToListAsync();
                MemoryStream reportCsv = new MemoryStream();
                var writer = new StreamWriter(reportCsv);
                var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(records);
                writer.Flush();
                reportCsv.Position = 0;
                mailRequest.MailAttachment = new Attachment(reportCsv,"report.csv","text/csv");
                await mailService.SendEmailAsync(mailRequest);
                await csv.DisposeAsync();
                return Ok(new { state = true, message = "Report Send Succesfully" });
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        private async Task<ActionResult> AttendeeReport(Report report)
        {
            try
            {
                var requester = await _context.Users.FindAsync(report.RequesterId);
                List<string> toEmails = new List<string>();
                toEmails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = toEmails.ToArray(),
                    Subject = $"Report Created Successfully for UserID:{report.UId}",
                    Body = "You can download your requested Report."
                };
                var records = await _context.Attendances
                    .Where(attend => attend.Date >= report.StartDate && attend.Date <= report.EndDate && attend.UId == report.UId && attend.Type == "Attend")
                    .Select(a => new { a.Id, a.Date, a.Type, a.InTime, a.OutTime, a.UId })
                    .ToListAsync();
                MemoryStream reportCsv = new MemoryStream();
                var writer = new StreamWriter(reportCsv);
                var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(records);
                writer.Flush();
                reportCsv.Position = 0;
                mailRequest.MailAttachment = new Attachment(reportCsv, "report.csv", "text/csv");
                await mailService.SendEmailAsync(mailRequest);
                await csv.DisposeAsync();
                return Ok(new { state = true, message = "Report Send Succesfully" });
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        private async Task<ActionResult> LeaveMasterReport(Report report)
        {
            try
            {
                var requester = await _context.Users.FindAsync(report.RequesterId);
                List<string> toEmails = new List<string>();
                toEmails.Add(requester.Email);
                MailRequest mailRequest = new MailRequest()
                {
                    ToEmails = toEmails.ToArray(),
                    Subject = $"Report Created Successfully for UserID:{report.UId}",
                    Body = "You can download your requested Report."
                };
                var records = await _context.LeaveDetails
                    .Where(leave => leave.Date >= report.StartDate && leave.Date <= report.EndDate && leave.SenderId == report.UId)
                    .Select(a => new { a.LeaveId, a.Date, a.Type, LeaveType=a.LeaveType.Type, a.SpecialNote, a.ApprovalId })
                    .ToListAsync();
                MemoryStream reportCsv = new MemoryStream();
                var writer = new StreamWriter(reportCsv);
                var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(records);
                writer.Flush();
                reportCsv.Position = 0;
                mailRequest.MailAttachment = new Attachment(reportCsv, "report.csv", "text/csv");
                await mailService.SendEmailAsync(mailRequest);
                await csv.DisposeAsync();
                return Ok(new { state = true, message = "Report Send Succesfully" });
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }
}
