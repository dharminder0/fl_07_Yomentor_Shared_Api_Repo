using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
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
        public  async Task<List<AttendanceResponse>> GetStudentsAttendance(AttendanceRequest request) {
            List < AttendanceResponse > obj=new List<AttendanceResponse> ();
       
            if (request == null) {
                return new List<AttendanceResponse>();
            }
            var response=  await  _attendanceRepository.GetStudentsAttendance(request);
            foreach (var item in response) {
                AttendanceResponse attendance = new AttendanceResponse();
                attendance.Id = item.Id;    
                attendance.StudentId = item.StudentId;  
                attendance.Status = item.Status;    
                attendance.Date = item.Date;
                if(item.UpdateDate== DateTime.MinValue)
                {
                    attendance.UpdateDate =DateTime.Now;
                }
                else
                {

                    attendance.UpdateDate = item.UpdateDate;
                }
                attendance.CreateDate = item.CreateDate;
                attendance.BatchId = item.BatchId;
                attendance.FirstName=item.FirstName;    
                attendance.LastName=item.LastName;  
                attendance.Phone=item.Phone;    
                obj.Add(attendance);

            }
            return obj; 
         
        }
       
    }
}
