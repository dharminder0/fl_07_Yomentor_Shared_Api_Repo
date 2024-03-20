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
        private readonly IBatchStudentsRepository _batchStudents;
        public AttendanceService(IAttendanceRepository attendanceRepository, IUserRepository userRepository, IBatchStudentsRepository batchStudents) {
            _attendanceRepository = attendanceRepository;
            _userRepository = userRepository;
            _batchStudents = batchStudents;
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

        public async Task<ActionMassegeResponse> BulkInsertAttendance(AttendanceV2 attendance) {
            try {
                //if(attendance.Id > 0)
                //{
                //    var response= await _attendanceRepository.BulkUpdateAttendance(attendance);  
                //    return new ActionMassegeResponse { Content= response,Message="Attendance_Updated!!",Response = true};
                //}
                var res = await _attendanceRepository.BulkInsertAttendance(attendance);
                return new ActionMassegeResponse { Content = res, Message = "Attendance_Inserted!!", Response = true };

            } catch (Exception ex) {
                return new ActionMassegeResponse { Content = null, Message = ex.Message, Response = true };
            }
        }
        public async Task<List<AttendanceResponse>> GetStudentsAttendance(AttendanceRequest request) {
            List<BatchStudents> batches=new List<BatchStudents>();
            if (request == null) {
                return new List<AttendanceResponse>();
            }

            List <AttendanceResponse> obj=new List<AttendanceResponse> ();       
            var response = await _attendanceRepository.GetStudentsAttendance(request);
       

                batches = _batchStudents.GetBatchStudentsbybatchId(request.BatchId).ToList();
          
         

            foreach (var item in response) {
                var info = await _userRepository.GetUser(item.StudentId);
                if (info != null) {
                    AttendanceResponse attendance = new AttendanceResponse {
                        Id = item.Id,
                        StudentId = item.StudentId,
                        Status = item.Status,
                        Date = item.Date,
                        UpdateDate = item.UpdateDate,
                        CreateDate = item.CreateDate,
                        BatchId = item.BatchId,
                        FirstName = info.Firstname,
                        LastName = info.Lastname,
                        Phone = info.Phone
                    };
                    obj.Add(attendance);
                }
            }

            foreach (var batch in batches) {
                var info = await _userRepository.GetUser(batch.StudentId);
                if (info != null) {
                    AttendanceResponse res = new AttendanceResponse {
                        Id = batch.Id,
                        StudentId = batch.Id,
                        CreateDate = batch.CreateDate,
                        UpdateDate = batch.UpdateDate,
                        BatchId = batch.BatchId,
                        FirstName = info.Firstname,
                        LastName = info.Lastname,
                        Phone = info.Phone
                    };
                    obj.Add(res);
                }
            }

            return obj;
        }


    }
}
