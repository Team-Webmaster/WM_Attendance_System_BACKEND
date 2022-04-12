using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Services
{
    public interface IJWTService
    {
        public string generateJwtToken(Login login);
        public string readClaimJwtToken(string token);
    }
}
