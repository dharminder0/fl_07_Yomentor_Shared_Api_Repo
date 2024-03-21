﻿using Autofac.Features.Scanning;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.DTOs;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Runtime.InteropServices.Marshalling;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Sevices.Concrete {
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        
        private readonly IGradeRepository _gradeRepository; 
        private readonly ISubjectRepository _subjectRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IUserRepository _userRepository;
        public BatchService(IBatchRepository batchRepository,  IGradeRepository gradeRepository, ISubjectRepository subjectRepository, IBatchStudentsRepository batchStudentsRepository,IUserRepository userRepository)
        {
            _batchRepository = batchRepository;         
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _batchStudentsRepository = batchStudentsRepository;
            _userRepository = userRepository;
        }
        public async Task<List<BatchDto>> BatchDetails(BatchRequest request)
        {
            if (request.Userid <= 0)
                throw new Exception("Teacher Id is blank!!!");

            try
            {
                var res = await _batchRepository.GetBatchDetailsbyId(request);
                if (res ==null) throw new Exception("Data is empty with this params");
                BatchDto batch = new BatchDto();
                List<BatchDto> batchDtos = new List<BatchDto>();    
                foreach(var row in res) {
                batch = new BatchDto();
                int BatchId = row.Id;
                IEnumerable<int> count = _batchRepository.CounterStudent(BatchId);
                int noofstudents = count.ElementAt(0);
                batch.ActualStudents = noofstudents;
                batch.BatchName = row.Name;
                batch.StartDate = row.StartDate;
                batch.UpdateDate = row.UpdateDate;
                batch.CreateDate = row.CreateDate;
                batch.Description = row.Description;
                batch.TuitionTime = row.TuitionTime;
                batch.ClassName = _gradeRepository.GetGradeName(row.GradeId);
                batch.SubjectName = _subjectRepository.GetSubjectName(row.SubjectId);
                batch.Fee = row.Fee;
                batch.StudentCount = row.StudentCount;
                batch.Status = System.Enum.GetName(typeof(Status), row.Status);
                batch.FeeType = System.Enum.GetName(typeof(FeeType), row.FeeType);
                batch.Id = row.Id;
                batch.StatusId = row.Status;
                batch.Days = row.Days != null ? ConvertToDays(row.Days).Select(day => day.ToString()).ToList() : null;
                    TeacherInformation teacher = new TeacherInformation();
                    if (request.UserType == 3) {
                        var teacherdetails = await _userRepository.GetUser(row.TeacherId);
                        teacher.Id = teacherdetails.Id;
                        teacher.FirstName = teacherdetails.Firstname;
                        teacher.LastName = teacherdetails.Lastname;
                        teacher.Phone = teacherdetails.Phone;
                        batch.TeacherInformation = teacher;
                    }
                  
                    batch.Days = row.Days != null ? ConvertToDays(row.Days).Select(day => day.ToString()).ToList() : null;
                    batchDtos.Add(batch);
                   
                }

                return batchDtos;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<ActionMassegeResponse> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest)
        {
            BatchDetailRequest obj = new BatchDetailRequest();
            obj.SubjectId = batchDetailRequest.SubjectId;
            obj.TeacherId = batchDetailRequest.TeacherId;
            obj.GradeId = batchDetailRequest.GradeId;
            obj.Days = JsonConvert.SerializeObject(batchDetailRequest.Days);
            obj.Date = batchDetailRequest.Date;
            obj.CreateDate = DateTime.Now;
            obj.Fee = batchDetailRequest.Fee;
            obj.FeeType = batchDetailRequest.FeeType;
            obj.Description = batchDetailRequest.Description;
            obj.Name = batchDetailRequest.Name;
            obj.NumberOfStudents = batchDetailRequest.NumberOfStudents;
            obj.ClassTime = batchDetailRequest.ClassTime;
            obj.Id = batchDetailRequest.Id;

            if (batchDetailRequest.Id > 0)
            {
                int Batchid = await _batchRepository.UpdateBatchDetails(obj);
                return new ActionMassegeResponse { Content = Batchid, Message = "Batch_Details_Updated successfully ", Response = true };
            }

            int id = await _batchRepository.InsertBatchDetails(obj);
            return new ActionMassegeResponse { Content = id, Message = "Batch_Created", Response = true };
        }




        private List<Days> ConvertToDays(string response) {
            response = response.Replace("[", "").Replace("]", "").Replace("\"", "");

            string[] numbers = response.Split(',');

            List<Days> days = new List<Days>();
            foreach (string num in numbers) {
                if (int.TryParse(num, out int dayNumber)) {
                    if (System.Enum.IsDefined(typeof(Days), dayNumber)) {
                        days.Add((Days)dayNumber);
                    }
                }
            }
            return days;
        }
        public List<BatchStudentDetailsDto> GetStudentDetailsbyBatchId(int batchId)
        {
            List<BatchStudentDetailsDto> listBatchStudentDetails = new List<BatchStudentDetailsDto>();

            var batchDetails = _batchRepository.GetBatchDetailsbybatchId(batchId);

            foreach (var batchItem in batchDetails)
            {
                var batchStudents = _batchStudentsRepository.GetBatchStudentsbybatchId(batchId);

                foreach (var batchStudent in batchStudents)
                {
                    var userDetails = _userRepository.GetStudentUser(new List<int> { batchStudent.StudentId });

                    foreach (var user in userDetails)
                    {
                        BatchStudentDetailsDto batchStudentDetailsDto = new BatchStudentDetailsDto();
                        batchStudentDetailsDto.BatchId = batchId;
                        batchStudentDetailsDto.SubjectName = batchItem.Name;
                        batchStudentDetailsDto.BatchId = batchItem.Id;
                        batchStudentDetailsDto.EnrollmentStatus = System.Enum.GetName(typeof(Enrollmentstatus), batchStudent.Enrollmentstatus);
                        batchStudentDetailsDto.enrollmentstatus = batchStudent.Enrollmentstatus;
                        batchStudentDetailsDto.StudentId = user.Id;
                        batchStudentDetailsDto.Address = user.Address;
                        batchStudentDetailsDto.Phone = user.Phone;
                        batchStudentDetailsDto.Name = user.Firstname.Replace(" ", "") + ' ' + user.Lastname.Replace(" ", "");
                        batchStudentDetailsDto.Email = user.Email == null ? null : user.Email.ToLower();

                        listBatchStudentDetails.Add(batchStudentDetailsDto);
                    }
                }
            }

            return listBatchStudentDetails;
        }
     
    }

}
