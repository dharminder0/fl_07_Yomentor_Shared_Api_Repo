using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
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
        private readonly IUserRepository _userRepository;   
        public AttendanceService(IAttendanceRepository attendanceRepository, IUserRepository userRepository) {
        _attendanceRepository = attendanceRepository;
            _userRepository = userRepository;
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
        public async Task<ActionMassegeResponse> BulkInsertAttendance(AttendanceV2 attendance)
        {
            try
            {
                if(attendance.Id > 0)
                {
                    var response= await _attendanceRepository.BulkUpdateAttendance(attendance);  
                    return new ActionMassegeResponse { Content= response,Message="Attendance_Updated!!",Response = true};
                }
                var res = await _attendanceRepository.BulkInsertAttendance(attendance);
                return new ActionMassegeResponse { Content = res, Message = "Attendance_Inserted!!", Response = true };

            }
            catch (Exception ex)
            {
                return new ActionMassegeResponse { Content = null, Message = ex.Message, Response = true };
            }
        }

        public  async Task<List<AttendanceResponse>> GetStudentsAttendance(AttendanceRequest request) {
            List < AttendanceResponse > obj=new List<AttendanceResponse> ();
       
            if (request == null) {
                return new List<AttendanceResponse>();
            }
            var response=  await  _attendanceRepository.GetStudentsAttendance(request);
            foreach (var item in response) {
                var info=  await _userRepository.GetUser(item.StudentId);
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
                attendance.FirstName=info.Firstname;    
                attendance.LastName= info.Lastname;  
                attendance.Phone= info.Phone;    
                obj.Add(attendance);

            }
            return obj; 
         
        }
       
    }
}
