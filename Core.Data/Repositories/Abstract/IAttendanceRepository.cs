using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IAttendanceRepository
    {

        Task<int> InsertAttendance(Attendance attendance);
        Task<int> UpdateAttendance(Attendance attendance);
        Task<IEnumerable<AttendanceResponse>> GetStudentsAttendance(AttendanceRequest attendanceRequest);
    }
}
