﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Data
{
    public partial class Hybrid_Attendance_SystemContext
    {

        [DbFunction("AttendHours", "dbo")]
        public static float AttendHours(int userId, string start_date, string end_date)
        {
            throw new NotSupportedException("This method can only be called from Entity Framework Core queries");
        }

        [DbFunction("BreakHours", "dbo")]
        public static float? BreakHours(int? userId, DateTime? start_date, DateTime? end_date)
        {
            throw new NotSupportedException("This method can only be called from Entity Framework Core queries");
        }

        [DbFunction("Test", "dbo")]
        public static int Test(int number)
        {
            throw new NotSupportedException();
        }

        [DbFunction("CalendarEventsByUser", "dbo")]
        public IQueryable<CalendarEventsByUserResult> CalendarEventsByUser(int user_id)
        {
            return FromExpression(() => CalendarEventsByUser(user_id));
        }

        [DbFunction("ShortCalendarEventsByUser", "dbo")]
        public IQueryable<ShortCalendarEventsByUser> ShortCalendarEventsByUser(int user_id)
        {
            return FromExpression(() => ShortCalendarEventsByUser(user_id));
        }

        protected void OnModelCreatingGeneratedFunctions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEventsByUserResult>().HasNoKey();
            modelBuilder.Entity<ShortCalendarEventsByUser>().HasNoKey();
        }
    }
}
