using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using WM_Attendance_System.Models;
using WM_Attendance_System.Services;

namespace WM_Attendance_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IMailService mailService;
        public EmailController(IMailService mailService)
        {
            this.mailService = mailService;
        }
        [HttpPost]
        public async Task<IActionResult> SendMail(MailRequest mailRequest)
        {
            try
            {
                await mailService.SendEmailAsync(mailRequest);
                return Ok(new { state = true, message = "Email send succesfully completed. Please check your Emails." });
            }
            catch(Exception e)
            {
                throw e.InnerException;
            }
        }
    }
}
