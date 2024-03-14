using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        public AttendanceService(IAttendanceRepository attendanceRepository) {
        _attendanceRepository = attendanceRepository;
        }
        public async Task<ActionMassegeResponse> InsertAttendance(Attendance attendance)
        {
            try
            {
                if (attendance.Id > 0)
                {
                    var response = await _attendanceRepository.UpdateAttendance(attendance);
                    return new ActionMassegeResponse { Content = response, Message = "Attendance_Updated!!", Response = true };
                }
                var res = await _attendanceRepository.InsertAttendance(attendance);
                return new ActionMassegeResponse { Content = res, Message = "Attendance_Inserted!!", Response = true };
            }
            catch (Exception ex)
            {
                return new ActionMassegeResponse { Content = null, Message = "ex.Message", Response = true };
            }
        }
    }
}
