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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBatchStudentsRepository _batchStudents;
        private readonly IMediaFileRepository _mediaFile;
        public AttendanceService(IAttendanceRepository attendanceRepository, IUserRepository userRepository, IBatchStudentsRepository batchStudents, IMediaFileRepository mediaFile) {
            _attendanceRepository = attendanceRepository;
            _userRepository = userRepository;
            _batchStudents = batchStudents;
            _mediaFile = mediaFile;
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
                var res = 0;
               await  _attendanceRepository.DeleteAttendance(attendance.BatchId, attendance.Date);
                foreach (var item in attendance.student_attendance) {
                    Attendance obj = new Attendance();
                    obj.UpdateDate = DateTime.Now;
                    obj.CreateDate = DateTime.Now;
                    obj.BatchId = attendance.BatchId;
                    obj.Date = attendance.Date;
                    obj.Status = item.Status;
                    obj.StudentId = item.StudentId;

                     res = await _attendanceRepository.InsertAttendance(obj);
     


                }
                return new ActionMassegeResponse { Content = res, Message = "Attendance_Inserted!!", Response = true };

            } catch (Exception ex) {
                return new ActionMassegeResponse { Content = null, Message = ex.Message, Response = true };
            }
        }
        public async Task<List<AttendanceResponse>> GetStudentsAttendance(AttendanceRequest request) {
            if (request == null) {
                return new List<AttendanceResponse>();
            }

            var response = await _attendanceRepository.GetStudentsAttendance(request);

            var studentIds = response.Select(r => r.StudentId).ToList();
            var batchStudents = _batchStudents.GetBatchStudentsbybatchId(request.BatchId)
                                              .Where(bs => !studentIds.Contains(bs.StudentId))
                                              .ToList();

            var obj = new List<AttendanceResponse>();
            foreach (var item in response) {
                var info = await _userRepository.GetUser(item.StudentId);
                var image = _mediaFile.GetImage(item.StudentId, Entities.DTOs.Enum.MediaEntityType.Users);
                if (info != null) {
                    var attendance = new AttendanceResponse {
                        Id = item.Id,
                        StudentId = item.StudentId,
                        Status = item.Status,
                        Date = item.Date,
                        UpdateDate = item.UpdateDate,
                        CreateDate = item.CreateDate,
                        BatchId = item.BatchId,
                        FirstName = info.Firstname,
                        LastName = info.Lastname,
                        Phone = info.Phone,
                        Image = image != null ? image.BlobLink : null


                    };
                    obj.Add(attendance);
                }
            }

            foreach (var batch in batchStudents) {
                var info = await _userRepository.GetUser(batch.StudentId);
                var image = _mediaFile.GetImage(batch.StudentId, Entities.DTOs.Enum.MediaEntityType.Users);
                if (info != null) {
                    var res = new AttendanceResponse {
                        Id = batch.Id,
                        StudentId = batch.StudentId,
                        CreateDate = batch.CreateDate,
                        UpdateDate = batch.UpdateDate,
                        BatchId = batch.BatchId,
                        FirstName = info.Firstname,
                        LastName = info.Lastname,
                        Phone = info.Phone,
                        Image = image != null ? image.BlobLink : null

                    };
                    obj.Add(res);
                }
            }

            return obj;
        }



    }
}
