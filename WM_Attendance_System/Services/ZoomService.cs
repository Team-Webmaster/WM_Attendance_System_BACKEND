using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WM_Attendance_System.Models;
using WM_Attendance_System.Settings;

namespace WM_Attendance_System.Services
{
    public class ZoomService:IZoomService
    {
        public readonly ZoomAPI zoomAPI;
        public ZoomService(IOptions<ZoomAPI> zoomAPI)
        {
            this.zoomAPI = zoomAPI.Value;
        }
        public async Task<MeetingLinks> getScheduledLinks(MeetingSettings meetingSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            byte[] symmetricKey = Encoding.ASCII.GetBytes(zoomAPI.APISecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = zoomAPI.APIKey,
                Expires = now.AddSeconds(300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var client = new RestClient($"https://api.zoom.us/v2/users/{zoomAPI.UserId}/meetings");
            var request = new RestRequest();
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { topic = meetingSettings.Topic, duration = meetingSettings.Duration, start_time = meetingSettings.StartTime, type = meetingSettings.Type });
            request.AddHeader("authorization", String.Format("Bearer {0}", tokenString));
            RestResponse restResponse = await client.PostAsync(request);
            var jObject = JObject.Parse(restResponse.Content);
            MeetingLinks meetingLinks = new MeetingLinks();
            meetingLinks.HostLink = (string)jObject["start_url"];
            meetingLinks.JoinLink = (string)jObject["join_url"];
            return meetingLinks;
        }
    }
}
