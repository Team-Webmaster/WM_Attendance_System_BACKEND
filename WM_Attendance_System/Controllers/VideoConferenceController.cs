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

        public VideoConferenceController(Hybrid_Attendance_SystemContext context, IZoomService zoomService)
        {
            _context = context;
            this.zoomService = zoomService;
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
        public async Task<IActionResult> PutVideoConference(int id, VideoConference videoConference)
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
            _context.VideoConferences.Add(videoConference);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVideoConference", new { id = videoConference.ConferenceId }, videoConference);
        }

        [HttpPost("zoom")]
        public async Task<ActionResult> CreateMeeting(MeetingSettings meetingSettings)
        {
            MeetingLinks meetingLinks = zoomService.getScheduledLinks(meetingSettings).Result;
            if(meetingLinks.HostLink is null || meetingLinks.JoinLink is null)
            {
                return BadRequest();
            }
            return Ok(new { state = true, data = meetingLinks });
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

        private bool VideoConferenceExists(int id)
        {
            return _context.VideoConferences.Any(e => e.ConferenceId == id);
        }
    }
}
