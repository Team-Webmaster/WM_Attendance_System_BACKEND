using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using WM_Attendance_System.Models;

namespace WM_Attendance_System.Data
{
    public partial class Hybrid_Attendance_SystemContext
    {
        private Hybrid_Attendance_SystemContextProcedures _procedures;

        public virtual Hybrid_Attendance_SystemContextProcedures Procedures
        {
            get
            {
                if (_procedures is null) _procedures = new Hybrid_Attendance_SystemContextProcedures(this);
                return _procedures;
            }
            set
            {
                _procedures = value;
            }
        }

        public Hybrid_Attendance_SystemContextProcedures GetProcedures()
        {
            return Procedures;
        }
    }

    public partial class Hybrid_Attendance_SystemContextProcedures
    {
        private readonly Hybrid_Attendance_SystemContext _context;

        public Hybrid_Attendance_SystemContextProcedures(Hybrid_Attendance_SystemContext context)
        {
            _context = context;
        }

        public virtual async Task<int> WorkingHoursAsync(int? user_id, DateTime? start_date, DateTime? end_date, OutputParameter<float?> workHours, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterworkHours = new SqlParameter
            {
                ParameterName = "workHours",
                Direction = ParameterDirection.InputOutput,
                Value = workHours?._value ?? Convert.DBNull,
                SqlDbType = SqlDbType.Real,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int,
            };

            var sqlParameters = new []
            {
                new SqlParameter
                {
                    ParameterName = "user_id",
                    Value = user_id ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "start_date",
                    Value = start_date ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Date,
                },
                new SqlParameter
                {
                    ParameterName = "end_date",
                    Value = end_date ?? Convert.DBNull,
                    SqlDbType = SqlDbType.Date,
                },
                parameterworkHours,
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [dbo].[WorkingHours] @user_id, @start_date, @end_date, @workHours OUTPUT", sqlParameters, cancellationToken);

            workHours.SetValue(parameterworkHours.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
