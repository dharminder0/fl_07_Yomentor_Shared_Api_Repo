using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class AssessmentsService : IAssessmentsService {
        private readonly IAssessmentsRepository _assessmentsRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IStudentAssessmentRepository _studentAssessmentRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        public AssessmentsService(IAssessmentsRepository assessmentsRepository, IBatchStudentsRepository batchStudentsRepository, IStudentAssessmentRepository studentAssessmentRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository, IMediaFileRepository mediaFileRepository) {
            _assessmentsRepository = assessmentsRepository;
            _batchStudentsRepository = batchStudentsRepository;
            _studentAssessmentRepository = studentAssessmentRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _mediaFileRepository = mediaFileRepository;
        }
        public async Task<ActionMessageResponse> InsertOrUpdateAssessmentsV2(AssessmentsRequest assessmentsRequest) {
            if (assessmentsRequest == null) {
                return new ActionMessageResponse { Success = false };
            }

            Assessments assessments = new Assessments();
            assessments.Title = assessmentsRequest.Title;
            assessments.Description = assessmentsRequest.Description;
            assessments.Id = assessmentsRequest.Id;
            assessments.TeacherId = assessmentsRequest.TeacherId;
            assessments.IsDeleted = assessmentsRequest.Isdeleted;
            assessments.Subjectid = assessmentsRequest.SubjectId;
            assessments.GradeId = assessmentsRequest.GradeId;
            assessments.IsFavorite = assessmentsRequest.Isfavorite;
            assessments.Createdate = assessmentsRequest.Createdate;
            assessments.Updatedate = assessmentsRequest.UpdatedDate;

            if (assessmentsRequest.Id == 0) {
                int insertedId = await _assessmentsRepository.InsertAssessments(assessments);
                try {
                    if (assessmentsRequest.uploadedFiles != null && assessmentsRequest.uploadedFiles.Any()) {
                        foreach (var item in assessmentsRequest.uploadedFiles) {
                            MediaFileRequest mediaFile = new MediaFileRequest();
                            mediaFile.FileName = item.FileName;
                            mediaFile.Bloblink = item.FileLink;
                            mediaFile.EntityId = insertedId;
                            mediaFile.EntityTypeId = Entities.DTOs.Enum.MediaEntityType.Assessment;
                            _mediaFileRepository.UpsertMediaFile(mediaFile);


                        }


                    }

                } catch (Exception) {

                    throw;
                }

                return new ActionMessageResponse { Content = insertedId, Message = "Assignment_created", Success = true };
            }

            int id = await _assessmentsRepository.UpdateAssessments(assessments);
            try {
                if (assessmentsRequest.uploadedFiles != null && assessmentsRequest.uploadedFiles.Any()) {
                    foreach (var item in assessmentsRequest.uploadedFiles) {
                        MediaFileRequest mediaFile = new MediaFileRequest();
                        mediaFile.FileName = item.FileName;
                        mediaFile.Bloblink = item.FileLink;
                        mediaFile.EntityId = id;
                        mediaFile.EntityTypeId = Entities.DTOs.Enum.MediaEntityType.Assessment;
                        _mediaFileRepository.UpsertMediaFile(mediaFile);


                    }


                }

            } catch (Exception) {

                throw;
            }
            return new ActionMessageResponse { Content = id, Message = "Assignment_Updated", Success = true };
        }
        public async Task<ActionMessageResponse> InsertOrUpdateAssessments(AssessmentsRequest assessmentsRequest) {
            if (assessmentsRequest == null) {
                return new ActionMessageResponse { Success = false };
            }

            Assessments assessments = new Assessments {
                Title = assessmentsRequest.Title,
                Description = assessmentsRequest.Description,
                Id = assessmentsRequest.Id,
                TeacherId = assessmentsRequest.TeacherId,
                IsDeleted = assessmentsRequest.Isdeleted,
                Subjectid = assessmentsRequest.SubjectId,
                GradeId = assessmentsRequest.GradeId,
                IsFavorite = assessmentsRequest.Isfavorite,
                Createdate = assessmentsRequest.Createdate,
                Updatedate = assessmentsRequest.UpdatedDate
            };

            int id = assessments.Id == 0
                ? await _assessmentsRepository.InsertAssessments(assessments)
                : await _assessmentsRepository.UpdateAssessments(assessments);

            try {
                if (assessmentsRequest.uploadedFiles != null && assessmentsRequest.uploadedFiles.Any()) {
                    await ProcessUploadedFiles(id, assessmentsRequest.uploadedFiles);
                }
            } catch (Exception) {
                throw;
            }

            string message = assessments.Id == 0 ? "Assignment_created" : "Assignment_Updated";
            return new ActionMessageResponse { Content = id, Message = message, Success = true };
        }

        private async Task ProcessUploadedFiles(int entityId, List<FileUploadResponse> uploadedFiles) {
            _mediaFileRepository.DeleteMediaFIle(entityId, (int)Entities.DTOs.Enum.MediaEntityType.Assessment);
            foreach (var item in uploadedFiles) {
                MediaFileRequest mediaFile = new MediaFileRequest {
                    FileName = item.FileName,
                    Bloblink = item.FileLink,
                    EntityId = entityId,
                    EntityTypeId = Entities.DTOs.Enum.MediaEntityType.Assessment
                };
                _mediaFileRepository.InsertInMediaFile(mediaFile);
            }
        }

        public  List<AssessmentResponse> GetAssessmentsList(int id) {
            if (id <= 0) {
                throw new ArgumentOutOfRangeException("id is null or blank");
            }

            List<AssessmentResponse> assessmentResponses = new List<AssessmentResponse>();
            List<FileUploadResponse> ros = new List<FileUploadResponse>();


            var item =  _assessmentsRepository.GetAssessments(id);


                AssessmentResponse obj = new AssessmentResponse()
;
                obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                obj.Id = item.Id;
                obj.TeacherId = item.TeacherId;
                obj.Title = item.Title;
                obj.Description = item.Description;
                obj.GradeId = item.GradeId;
                obj.IsFavorite = item.IsFavorite;
                obj.Id = item.Id;
                obj.CreateDate = item.Createdate;
                obj.UpdateDate = item.Updatedate;
                obj.MaxMark = item.Maxmark;
                obj.SubjectId = item.Subjectid;


                try {
                    var files = _mediaFileRepository.GetEntityMediaFile(item.Id, Entities.DTOs.Enum.MediaEntityType.Assessment);

                    foreach (var fileItem in files) {
                        FileUploadResponse fileUpload = new FileUploadResponse {
                            FileLink = fileItem.BlobLink,
                            FileName = fileItem.FileName,
                            FileIdentifier=fileItem.FileName,
                        };
                       ros.Add(fileUpload); 
                    obj.UploadFiles=ros;


                    }
                assessmentResponses.Add(obj);
                return assessmentResponses;

                } catch (Exception) {


                }

            return null;
            }
              

                


            
    
        
        public async Task<List<AssessmentResponse>> GetAssessmentsAllList(StudentProgressRequestV2 request)
        {
            try
            {
                List<AssessmentResponse> res = new List<AssessmentResponse>();
                var response = await _assessmentsRepository.GetAssessmentsAllList(request);
            
                foreach (var item in response) {
                    AssessmentResponse obj = new AssessmentResponse();
                    obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                    obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                    obj.Id = item.Id;
                    obj.TeacherId = item.TeacherId;
                    obj.Title = item.Title;
                    obj.Description = item.Description;
                    obj.GradeId = item.GradeId; 
                    obj.IsFavorite = item.IsFavorite;
                    obj.Id = item.Id;
                    obj.CreateDate = item.Createdate;
                    obj.UpdateDate = item.Updatedate;
                    obj.MaxMark= item.Maxmark;  
                    obj.SubjectId = item.Subjectid; 
                    res.Add(obj);

                }

                return res;


            } catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ActionMassegeResponse> AssignStudentAssessment(StudentAssessmentRequestV2 requestV2) {
             int res=0;
            if (requestV2== null ) {
                return new ActionMassegeResponse { Response = false };
            }

            var response = _batchStudentsRepository.GetBatchStudentsbybatchId(requestV2.BatchId);
            if (response == null || !response.Any()) {
                return new ActionMassegeResponse { Message = "No students found for the given batch.", Response = false };
            }
            foreach (var item in response) {
                StudentAssessment student = new StudentAssessment
                {
                    Id = item.Id,   
                    Status = requestV2.Status,
                    AssessmentId = requestV2.AssessmentId,
                    BatchId = requestV2.BatchId,
                    StudentId = item.StudentId,
                    Marks = requestV2.Marks,
                };
                 res = await _studentAssessmentRepository.InsertStudentAssessment(student);
            }


            return new ActionMassegeResponse { Message = "Assigned_Successfully", Response = true, Content = res };
        }

        public async Task<List<AssessmentResponse>> GetAssessmentByBatch(ListRequest request) {
            if (request.BatchId == 0) {
                return new List<AssessmentResponse> { };
            }
            List<AssessmentResponse> res = new List<AssessmentResponse>();
            var response = await _assessmentsRepository.GetAssessmentsByBatch(request);
            foreach (var item in response) {
                AssessmentResponse obj = new AssessmentResponse();
                obj.Id = item.Id;
                obj.TeacherId = item.TeacherId;
                obj.Title = item.Title;
                obj.Description = item.Description;
                obj.GradeId = item.GradeId;
                obj.IsFavorite = item.IsFavorite;
                obj.Id = item.Id;
                obj.CreateDate = item.Createdate;
                obj.UpdateDate = item.Updatedate;
                obj.AssignedDate = item.AssignedDate;
                res.Add(obj);

            }

            return res;
        }

    }
}
