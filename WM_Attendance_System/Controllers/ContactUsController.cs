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
    public class ContactUsController : ControllerBase
    {
        private readonly Hybrid_Attendance_SystemContext _context;
	private readonly IMailService mailService;

        public ContactUsController(Hybrid_Attendance_SystemContext context, IMailService mailService)
        {
            _context = context;
	    this.mailService = mailService;
        }

        // POST: api/ContactUs
        [HttpPost]
        public async Task<IActionResult> ContactUs(ContactUs contactUs)
        {
            List<string> adminsEmails = new List<string>();
            List<string> senderEmail = new List<string>();
	    var admins = await _context.Users.Where(u=>u.Type==0).ToListAsync();
            foreach (var admin in admins)
            {
                adminsEmails.Add(admin.Email);
            }
	    var sender = await _context.Users.FindAsync(contactUs.UId);
	    senderEmail.Add(sender.Email);
            MailRequest mailRequestForAdmins = new MailRequest()
            {
                ToEmails=adminsEmails.ToArray(),
                Subject=$"{sender.Name} Wants to Contact You.",
                Body=$"{sender.Name} asked {contactUs.Message}. Here her/his contact details... Email:{contactUs.Email}"
            };
            await mailService.SendEmailAsync(mailRequestForAdmins);
            MailRequest mailRequestForSender = new MailRequest()
            {
                ToEmails = senderEmail.ToArray(),
                Subject = $"Dear {sender.Name} Your message is sends to admin panel",
                Body = $"Here is your contact details... Your reason:{contactUs.Message}, Your contact no:{contactUs.Telephone}, Your email:{contactUs.Email}."
            };
            await mailService.SendEmailAsync(mailRequestForSender);
            return Ok();
        }

    }
}
