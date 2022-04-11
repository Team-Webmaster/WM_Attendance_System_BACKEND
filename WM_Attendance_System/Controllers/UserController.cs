﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.EntityFrameworkCore;
using WM_Attendance_System.Data;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IFaceService faceService;

        public UserController(Hybrid_Attendance_SystemContext context, IWebHostEnvironment hostEnvironment, IFaceService faceService)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
            this.faceService = faceService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUserTables()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/User/5
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

        //PUT: api/User/changepassword/5
        [HttpPut("changepassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id,ChangePassword changePassword)
        {
            if (!UserTableExists(id))
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (!BCrypt.Net.BCrypt.Verify(changePassword.CurrentPassword, user.Password))
            {
                return BadRequest(new { state = false, message = "Old password incorrect." });
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { state = true, message = "Password changed succefully completed." });
            }
            catch(DbUpdateConcurrencyException)
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
        }

        //PUT: api/User/forgotpassword/5
        [HttpPut("forgotpassword/{id}")]
        public async Task<IActionResult> ForgotPassword(int id, ForgotPassword forgotPassword)
        {
            if (!UserTableExists(id))
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            user.Password = BCrypt.Net.BCrypt.HashPassword(forgotPassword.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { state = true, message = "Password changed succefully completed." });
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
        }

        // PUT: api/User/5
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

        //POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult<PendingUser>> PostPendingUserTable([FromForm]PendingUser pendingUser)
        {
            var blackListedUser = await _context.BlackListedEmails.FindAsync(pendingUser.Email);
            if (blackListedUser is not null)
            {
                return Ok(new { state = false, message = "This Email is Blacklisted. Please use another email for registration." });
            }
            var isPendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == pendingUser.Email);
            if(isPendingUser is not null)
            {
                return Ok(new { state = false, message = "This email is already in approving process. Try again with another email." });
            }
            var isUser = await _context.Users.SingleOrDefaultAsync(x => x.Email == pendingUser.Email);
            if(isUser is not null)
            {
                return Ok(new { state = false, message = "This email is already registered. Try again with another email." });
            }
            string imgName = await SaveImage(pendingUser.ProfilePicture);
            IFaceClient faceClient = faceService.Authenticate();
            var addedFaceToFaceList=await faceService.AddFaceToFaceList(faceClient,imgName);
            if(addedFaceToFaceList is null)
            {
                return Ok(new { state = false, message = "Uploaded image quality is not enough" });
            }
            pendingUser.ProfilePic = await SaveImage(pendingUser.ProfilePicture);
            pendingUser.Password = BCrypt.Net.BCrypt.HashPassword(pendingUser.Password);
            _context.PendingUsers.Add(pendingUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostPendingUserTable", new { id = pendingUser.PendingUserId }, pendingUser);
        }

        //POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> PostUserTable(Login login)
        {
            var User = await _context.Users.SingleOrDefaultAsync(x=> x.Email == login.Email);
            if (User is null)
            {
                var PendingUser = await _context.PendingUsers.SingleOrDefaultAsync(x => x.Email == login.Email);
                if (PendingUser is null)
                {
                    return Ok(new { state = false, message = "Email not found" });
                }
                return Ok(new { state = false, message = "Your account is in approving process. Please Try again later." });
            }
            if (BCrypt.Net.BCrypt.Verify(login.Password, User.Password))
            {
                return Ok(new { state = true, data = User });
            }
            return Ok(new { state = false, message = "Password Incorrect" });

        }

        // DELETE: api/User/5
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

        private async Task<string> SaveImage(IFormFile formFile)
        {
            string imgName = new String(Path.GetFileNameWithoutExtension(formFile.FileName).Take(10).ToArray()).Replace(' ','-');
            imgName = imgName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(formFile.FileName);
            string imgPath = Path.Combine(hostEnvironment.ContentRootPath, "Images", imgName);
            var fileStream = new FileStream(imgPath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
            await fileStream.DisposeAsync();
            return imgName;
        }

        private bool UserTableExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
