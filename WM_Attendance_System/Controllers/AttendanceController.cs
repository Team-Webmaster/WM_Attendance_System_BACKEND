using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.EntityFrameworkCore;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
        private readonly IFaceService faceService;
        private readonly IWebHostEnvironment hostEnvironment;

        public AttendanceController(Hybrid_Attendance_SystemContext context, IFaceService faceService, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.faceService = faceService;
            this.hostEnvironment = hostEnvironment;
        }

        // GET: api/Attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            return await _context.Attendances.ToListAsync();
        }

        // GET: api/Attendance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
            {
                return NotFound();
            }

            return attendance;
        }

        // PUT: api/Attendance/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return BadRequest();
            }

            _context.Entry(attendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
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

        // POST: api/Attendance
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendance", new { id = attendance.Id }, attendance);
        }

        // DELETE: api/Attendance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Add")]
        public async Task<IActionResult> MarkAttendance([FromForm]Attendance attendance)
        {
            IFaceClient faceClient = faceService.Authenticate();
            var result = await faceService.IdentifyFaceList(faceClient, await SaveImage(attendance.FaceImage));
            if(!result.Any())
            {
                return Ok(new { state = false, message = "Face not matched" });
            }
            return Ok(new { state = true, message = "Face matched", data = result });
        }

        private async Task<MemoryStream> SaveImage(IFormFile formFile)
        {
            MemoryStream image = new MemoryStream();
            await formFile.CopyToAsync(image);
            return image;
        } 
        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
