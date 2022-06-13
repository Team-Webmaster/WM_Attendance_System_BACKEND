using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoConferenceController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
        private readonly IZoomService zoomService;
        private readonly IMailService mailService;

        public VideoConferenceController(Hybrid_Attendance_SystemContext context, IZoomService zoomService, IMailService mailService)
        {
            _context = context;
            this.zoomService = zoomService;
            this.mailService = mailService;
        }

        // GET: api/VideoConference
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoConference>>> GetVideoConferences()
        {
            return await _context.VideoConferences.ToListAsync();
        }

        // GET: api/VideoConference/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoConference>> GetVideoConference(int id)
        {
            var videoConference = await _context.VideoConferences.FindAsync(id);

            if (videoConference == null)
            {
                return NotFound();
            }

            return videoConference;
        }

        // PUT: api/VideoConference/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideoConference(string id, VideoConference videoConference)
        {
            if (id != videoConference.ConferenceId)
            {
                return BadRequest();
            }

            _context.Entry(videoConference).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoConferenceExists(id))
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

        // POST: api/VideoConference
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<VideoConference>> PostVideoConference(VideoConference videoConference)
        {
            MeetingSettings meetingSettings = new MeetingSettings();
            meetingSettings.Topic = $"Meeting ID: {videoConference.ConferenceId}";
            meetingSettings.Duration = "120";
            meetingSettings.StartTime = $"{videoConference.Date}T{videoConference.Time}";
            meetingSettings.Type = "2";
            MeetingLinks meetingLinks = zoomService.getScheduledLinks(meetingSettings).Result;
            if (meetingLinks.HostLink is null || meetingLinks.JoinLink is null)
            {
                return BadRequest();
            }
            _context.VideoConferences.Add(videoConference);
            await _context.SaveChangesAsync();
            List<string> participantsEmails = new List<string>();
            List<string> hostEmail = new List<string>();
            var videoConfHasUser = new VideoConferenceHasUser();
            videoConfHasUser.ConferenceId = videoConference.ConferenceId;
            foreach (var userId in videoConference.Participants)
            {
                var user = await _context.Users.FindAsync(userId);
                participantsEmails.Add(user.Email);
                videoConfHasUser.UserId = userId;
                _context.VideoConferenceHasUsers.Add(videoConfHasUser);
		await _context.SaveChangesAsync();
            }
            videoConfHasUser.UserId = videoConference.HostId;
            _context.VideoConferenceHasUsers.Add(videoConfHasUser);
	    await _context.SaveChangesAsync();
            if (videoConference.SchedulerId != videoConference.HostId)
            {
                videoConfHasUser.UserId = videoConference.SchedulerId;
                _context.VideoConferenceHasUsers.Add(videoConfHasUser);
                var scheduler = await _context.Users.FindAsync(videoConference.SchedulerId);
                participantsEmails.Add(scheduler.Email);
            }
            await _context.SaveChangesAsync();
            var host = await _context.Users.FindAsync(videoConference.HostId);
            hostEmail.Add(host.Email);
            MailRequest mailRequestForParticipants = new MailRequest()
            {
                ToEmails=participantsEmails.ToArray(),
                Subject=$"Meeting Scheduled with ID:{videoConference.ConferenceId}",
                Body=$"To Join scheduled meeting use this link:{meetingLinks.JoinLink}"
            };
            await mailService.SendEmailAsync(mailRequestForParticipants);
            MailRequest mailRequestForHost = new MailRequest()
            {
                ToEmails = hostEmail.ToArray(),
                Subject = $"Meeting Scheduled with ID:{videoConference.ConferenceId}",
                Body = $"To start scheduled meeting use this link:{meetingLinks.HostLink}"
            };
            await mailService.SendEmailAsync(mailRequestForHost);
            return Ok(new { message="Video conference scheduled successfully completed." });
        }

        // DELETE: api/VideoConference/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideoConference(int id)
        {
            var videoConference = await _context.VideoConferences.FindAsync(id);
            if (videoConference == null)
            {
                return NotFound();
            }

            _context.VideoConferences.Remove(videoConference);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool VideoConferenceExists(string id)
        {
            return _context.VideoConferences.Any(e => e.ConferenceId == id);
        }
    }
}
