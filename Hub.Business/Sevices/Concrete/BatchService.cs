﻿using Autofac.Features.Scanning;
using Azure.Core;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.DTOs;
using Core.Business.Entities.RequestModels;
using Core.Business.Services.Abstract;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.AccessControl;
using static Core.Business.Entities.DTOs.Enum;
using static Slapper.AutoMapper;

namespace Core.Business.Sevices.Concrete {
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        
        private readonly IGradeRepository _gradeRepository; 
        private readonly ISubjectRepository _subjectRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFavouriteBatchRepository _favouriteBatchRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserService _user;
        public BatchService(IBatchRepository batchRepository,  IGradeRepository gradeRepository, ISubjectRepository subjectRepository, IBatchStudentsRepository batchStudentsRepository,IUserRepository userRepository, IFavouriteBatchRepository favouriteBatchRepository, IMediaFileRepository mediaFileRepository, IAddressRepository addressRepository, IUserService user)
        {
            _batchRepository = batchRepository;         
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _batchStudentsRepository = batchStudentsRepository;
            _userRepository = userRepository;
            _favouriteBatchRepository= favouriteBatchRepository;  
            _mediaFileRepository= mediaFileRepository;
            _addressRepository = addressRepository;
            _user = user;
        }
        public async Task<List<BatchDto>> BatchDetails(BatchRequest request)
        {
            if (request.UserId <= 0)
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
                    try {
                       
                            batch.IsFavourite = row.IsFavourite;   
                       
                        if (request.UserType == 3) {
                          var enrollmentstatus  =await  _batchStudentsRepository.GetEnrollmentStatus(BatchId, request.UserId);
                            if (enrollmentstatus != null) {
                                batch.Enrollmentstatus = System.Enum.GetName(typeof(Enrollmentstatus), enrollmentstatus.Enrollmentstatus);
                                batch.EnrollmentstatusId = enrollmentstatus.Enrollmentstatus;
                            }
                        }


                    } catch (Exception) {

                        throw;
                    }

                 
                int noofstudents = count.ElementAt(0);
                batch.ActualStudents = noofstudents;
                batch.BatchName = row.Name;
                batch.StartDate = row.StartDate;
                batch.UpdateDate = row.UpdateDate;
                batch.CreateDate = row.CreateDate;
                batch.Description = row.Description;
                batch.TuitionTime = row.TuitionTime;
                batch.GradeName = _gradeRepository.GetGradeName(row.GradeId);
                batch.SubjectName = _subjectRepository.GetSubjectName(row.SubjectId);
                batch.GradeId= row.GradeId; 
                batch.SubjectId= row.SubjectId; 
                batch.Fee = row.Fee;
                batch.StudentCount = row.StudentCount;
                batch.Status = System.Enum.GetName(typeof(BatchStatus), row.Status);
                batch.FeeType = System.Enum.GetName(typeof(FeeType), row.FeeType);
                batch.Id = row.Id;
                batch.StatusId = row.Status;
                batch.Days = row.Days != null ? ConvertToDays(row.Days).Select(day => day.ToString()).ToList() : null;
                    TeacherInformation teacher = new TeacherInformation();
                    if (request.UserType == 3) {
                        var teacherdetails = await _userRepository.GetUser(row.TeacherId);
                        if (teacherdetails != null) {
                            teacher.Id = teacherdetails.Id;
                            teacher.FirstName = teacherdetails.FirstName;
                            teacher.LastName = teacherdetails.LastName;
                            teacher.Phone = teacherdetails.Phone;
                            batch.TeacherInformation = teacher;
                        }
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
                       // batchStudentDetailsDto.Address = user.Address;
                        batchStudentDetailsDto.Phone = user.Phone;
                        batchStudentDetailsDto.Name = user.FirstName.Replace(" ", "") + ' ' + user.LastName.Replace(" ", "");
                        batchStudentDetailsDto.Email = user.Email == null ? null : user.Email.ToLower();
                        try {
                            var media = _mediaFileRepository.GetImage(user.Id, MediaEntityType.Users);
                            if (media != null) {
                                batchStudentDetailsDto.Image = media.BlobLink;
                            }

                        } catch (Exception) {

                          
                        }
                        var addressInfo = _addressRepository.GetUserAddress(user.Id);
                        if (addressInfo != null) {
                            Address address = new Address();
                            address.Address1 = addressInfo.Address1;
                            address.Address2 = addressInfo.Address2;
                            address.UserId = addressInfo.UserId;
                            address.StateId = addressInfo.StateId;
                            address.Latitude = addressInfo.Latitude;
                            address.Longitude = addressInfo.Longitude;
                            address.City = addressInfo.City;
                            address.IsDeleted = addressInfo.IsDeleted;
                            address.Id = addressInfo.Id;
                            address.Pincode = addressInfo.Pincode;
                            address.UpdateDate = addressInfo.UpdateDate;
                            try {
                                var stateName = _addressRepository.GetState(address.StateId);
                                address.StateName = stateName.Name;

                            } catch (Exception) {


                            }
                            batchStudentDetailsDto.UserAddress = address;

                        }

                        listBatchStudentDetails.Add(batchStudentDetailsDto);
                    }
                }
            }

            return listBatchStudentDetails;
        }
        public  async  Task<ActionMassegeResponse> UpdateBatchStatus(int batchStatus, int batchId) {
           bool response=   await _batchRepository.UpdateBatchStatus(batchStatus, batchId);
            return new ActionMassegeResponse { Content = response, Message = "Updated_successfully", Response = true };
        }
        public async Task<ActionMassegeResponse> UpdateEnrollmentStatus(int status, int Id, int batchId) {
            bool response = await _batchStudentsRepository.UpdateEnrollmentStatus(status, Id,batchId);
            try {
                _user.PushNotifications(NotificationType.enrollment_status_update, Id, batchId);
                

            } catch (Exception) {

              
            }
            return new ActionMassegeResponse { Content = response, Message = "Updated_successfully", Response = true };

        }
        public async Task<ActionMassegeResponse> AssignBatchStudents(BatchStudentsRequest request) {
            int res = 0;
            if (request == null) {
                return new ActionMassegeResponse { Response = false };
            }

            var response = await  _batchStudentsRepository.DeleteBatchStudents(request.BatchId,request.CreateDate);

            foreach (var item in request.student_Info) {
                BatchStudents obj = new BatchStudents();
                obj.UpdateDate = DateTime.Now;
                obj.BatchId = request.BatchId;
                obj.CreateDate = request.CreateDate;
                obj.Enrollmentstatus = item.Status;
                obj.StudentId = item.StudentId;

                res = await _batchStudentsRepository.InsertBatchStudent(obj);


                try {
                    int teacherid = _batchStudentsRepository.GetTeacherId(request.BatchId);
                    _user.PushNotifications(NotificationType.student_enrolled, teacherid, item.StudentId);
                } catch (Exception) {


                }



            }
            return new ActionMassegeResponse { Content = res, Message = "Assigned Successfully !!", Response = true };

        }
       public async  Task<ActionMassegeResponse> InsertOrUpdateFavouriteBatch(FavouriteBatchRequest batch) {
        
            if (batch == null) {
                return new ActionMassegeResponse { Response = false };
            }
            
                FavouriteBatch obj = new FavouriteBatch();
                obj.Id = batch.Id;
                obj.EntityTypeId = batch.EntityTypeId;
                obj.EntityType = batch.EntityType;
                obj.IsFavourite = batch.IsFavourite;
                obj.UserId = batch.UserId;
                obj.CreatedDate= DateTime.Now;            
               int  res = await _favouriteBatchRepository.InsertOrUpdateFavouriteBatch(obj);
            try {
                BatchStudents batchStudents=new BatchStudents();
                batchStudents.BatchId = batch.EntityTypeId;
                batchStudents.StudentId = batch.UserId;
                batchStudents.CreateDate = DateTime.Now;
                batchStudents.Enrollmentstatus = 0;
                batchStudents.IsDeleted= false; 
                batchStudents.UpdateDate = DateTime.Now;    
               await  _batchStudentsRepository.InsertBatchStudent(batchStudents);

            } catch (Exception) {

                throw;
            }
            return new ActionMassegeResponse { Content = res, Message = " favourite_Batch_Assigned Successfully ", Response = true };

        }
           

        public async Task<ActionMassegeResponse> UpdateFavouriteStatus(int userId, int entityId) {
            bool isresoponse=false;
           isresoponse=await  _favouriteBatchRepository.UpdateStatus(userId, entityId);
            return new ActionMassegeResponse { Response = true, Content = isresoponse, Message = "Updated Successfully" };

        }
        public async Task<List<BatchDto>> BatchDetails(BatchRequestV2 request) {
            if (request.teacherId <= 0)
                throw new Exception("Teacher Id is blank!!!");

            try {
                var res = await _batchRepository.GetBatchDetailsbyId(request);
                if (res == null) throw new Exception("Data is empty with this params");
                BatchDto batch = new BatchDto();
                List<BatchDto> batchDtos = new List<BatchDto>();
                foreach (var row in res) {
                    batch = new BatchDto();
                    int BatchId = row.Id;
                    IEnumerable<int> count = _batchRepository.CounterStudent(BatchId);
                    try {

                    
                       
                            var enrollmentstatus = await _batchStudentsRepository.GetEnrollmentStatus(BatchId, request.StudentId);
                            batch.Enrollmentstatus = System.Enum.GetName(typeof(Enrollmentstatus), enrollmentstatus.Enrollmentstatus);
                        batch.EnrollmentstatusId=enrollmentstatus.Enrollmentstatus;
                        var favBatch=await  _favouriteBatchRepository.GetFavouriteStatus(request.StudentId, BatchId);
                        batch.IsFavourite = favBatch.IsFavourite;
                        


                    } catch (Exception) {

                        
                    }
                  

                    int noofstudents = count.ElementAt(0);
                    batch.ActualStudents = noofstudents;
                    batch.BatchName = row.Name;
                    batch.StartDate = row.StartDate;
                    batch.UpdateDate = row.UpdateDate;
                    batch.CreateDate = row.CreateDate;
                    batch.Description = row.Description;
                    batch.TuitionTime = row.TuitionTime;
                    batch.GradeName = _gradeRepository.GetGradeName(row.GradeId);
                    batch.SubjectName = _subjectRepository.GetSubjectName(row.SubjectId);
                    batch.GradeId = row.GradeId;
                    batch.SubjectId = row.SubjectId;
                    batch.Fee = row.Fee;
                    batch.StudentCount = row.StudentCount;
                    batch.Status = System.Enum.GetName(typeof(BatchStatus), row.Status);
                    batch.FeeType = System.Enum.GetName(typeof(FeeType), row.FeeType);
                    batch.Id = row.Id;
                    batch.StatusId = row.Status;
                    batch.Days = row.Days != null ? ConvertToDays(row.Days).Select(day => day.ToString()).ToList() : null;
                    TeacherInformation teacher = new TeacherInformation();
                    
                        var teacherdetails = await _userRepository.GetUser(row.TeacherId);
                        if (teacherdetails != null) {
                            teacher.Id = teacherdetails.Id;
                            teacher.FirstName = teacherdetails.FirstName;
                            teacher.LastName = teacherdetails.LastName;
                            teacher.Phone = teacherdetails.Phone;
                            batch.TeacherInformation = teacher;
                        }
                    

                    batch.Days = row.Days != null ? ConvertToDays(row.Days).Select(day => day.ToString()).ToList() : null;

                    batchDtos.Add(batch);


                }

                return batchDtos;
            } catch (Exception ex) {
                return null;
            }
        }
        public bool DeleteBatch(int id) {
            return _batchRepository.DeleteBatch(id);
        }
    }
}

