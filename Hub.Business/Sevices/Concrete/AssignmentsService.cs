using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;

namespace Core.Business.Sevices.Concrete {
    public class AssignmentsService : IAssignmentsService {
        private readonly IAssignmentsRepository _assignmentsRepo;
        private readonly IStudentAssignmentsRepository _studentAssignmentsRepo;
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IGradeRepository _gradeRepository;
        public  readonly  ISubjectRepository _subjectRepository; 
        private readonly IMediaFileRepository _mediaFileRepository;
        public AssignmentsService(IAssignmentsRepository assignmentsRepo, IStudentAssignmentsRepository studentAssignmentsRepo, IBatchRepository batchRepository, IBatchStudentsRepository batchStudentsRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository, IMediaFileRepository mediaFileRepository) {
            _assignmentsRepo = assignmentsRepo;
            _studentAssignmentsRepo = studentAssignmentsRepo;
            _batchRepository = batchRepository;
            _batchStudentsRepository = batchStudentsRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
            _mediaFileRepository = mediaFileRepository;
        }
        public async Task<ActionMassegeResponse> InsertOrUpdateAssignmentsV2(AssignmentsRequest assignmentsRequest) {
            if (assignmentsRequest == null) {
                return new ActionMassegeResponse { Response = false };
            }
            Assignments assignments = new Assignments();
            assignments.Title = assignmentsRequest.Title;
            assignments.Description = assignmentsRequest.Description;
            assignments.Id = assignmentsRequest.Id;
            assignments.Teacherid = assignmentsRequest.TeacherId;
            assignments.Isdeleted = assignmentsRequest.IsDeleted;
            assignments.Subjectid = assignmentsRequest.SubjectId;
            assignments.GradeId = assignmentsRequest.GradeId;
            assignments.Isfavorite = assignmentsRequest.IsFavorite;
            if (assignmentsRequest.Id == 0) {
                int InsertedId = await _assignmentsRepo.InsertAssignment(assignments);
                return new ActionMassegeResponse { Content = InsertedId, Message = "Assignment_created", Response = true };
            }
            int id = await _assignmentsRepo.UpdateAssignment(assignments);
            return new ActionMassegeResponse { Content = id, Message = "Assignment_Updated", Response = true };
        }

        public async Task<List<AssignmentsResponse>> GetAssignment(int id) {

      
                List<AssignmentsResponse> assessmentResponses = new List<AssignmentsResponse>();  

                List<FileUploadResponse> UploadFiles = new List<FileUploadResponse>();
                var item = await _assignmentsRepo.GetAssignments(id);
            if (item != null) {

                AssignmentsResponse obj = new AssignmentsResponse();
                obj.Id = item.Id;
                obj.Title = item.Title;
                obj.Description = item.Description;
                obj.UpdateDate = item.UpdateDate;
                obj.CreateDate = item.CreateDate;
                obj.AssignedDate = item.AssignedDate;
                obj.GradeId = item.GradeId;
                obj.SubjectId=item.Subjectid;
                obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                try {
                    var files = _mediaFileRepository.GetEntityMediaFile(id, Entities.DTOs.Enum.MediaEntityType.Assignment);
                    foreach (var item1 in files) {
                        FileUploadResponse fileUpload = new FileUploadResponse();
                        fileUpload.FileLink = item1.BlobLink;
                        fileUpload.FileName = item1.FileName;
                        UploadFiles.Add(fileUpload);
                        obj.UploadedFiles = UploadFiles;

                    }
                } catch (Exception) {


                }

                assessmentResponses.Add(obj);
                return assessmentResponses;
            }
            return null;








            }

        public async Task<List<AssignmentsResponse>> GetAllAssignments(StudentProgressRequestV2 request) {
            try {
                var response = await _assignmentsRepo.GetAllAssignments(request);
                List <AssignmentsResponse> res = new List<AssignmentsResponse>();  


                foreach (var item in response) {
                    AssignmentsResponse obj = new AssignmentsResponse();

                    obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                    obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                    obj.SubjectId = item.Subjectid;
                    obj.TeacherId = item.Teacherid;
                    obj.Title = item.Title;
                    obj.Description = item.Description;
                    obj.IsFavorite = item.Isfavorite;
                    obj.GradeId = item.GradeId;
                    obj.Id = item.Id;
                    obj.CreateDate = item.CreateDate;
                    obj.UpdateDate = item.UpdateDate;
                    obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                    obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);

                    res.Add(obj);

                }

                return res;
            } catch (Exception ex) {
                return null;
            }
        }
        public async Task<ActionMassegeResponse> AssignStudentAssignments(StudentAssignmentsRequestV2 request) {
            int res = 0;
            if (request == null) {
                return new ActionMassegeResponse { Response = false };
            }

            var response = _batchStudentsRepository.GetBatchStudentsbybatchId(request.BatchId);
            if (response == null || !response.Any()) {
                return new ActionMassegeResponse { Message = "No students found for the given batch.", Response = false };
            }


            foreach (var item in response) {
                Student_Assignments student = new Student_Assignments { 
               Status = request.Status,
               AssignmentId = request.AssignmentId,
               BatchId = request.BatchId,
               StudentId = item.StudentId,
               Id = item.Id
            };
               
                 res = await _studentAssignmentsRepo.InsertStudentAssignment(student);
            }

            
            return new ActionMassegeResponse {  Message = "Assigned_Successfully", Response = true,Content=res };
        }

       public  async Task<List<AssignmentsResponse>> GetAssignmentsByBatch(ListRequest request) {
            if (request.BatchId == 0) {
                return new List<AssignmentsResponse> { };
            }
            List<AssignmentsResponse> res = new List<AssignmentsResponse>();
          var response=await   _assignmentsRepo.GetAssignmentsByBatch(request);
            foreach (var item in response) {
                AssignmentsResponse obj = new AssignmentsResponse();
                obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                obj.SubjectId = item.Subjectid; 
                obj.TeacherId=item.Teacherid;   
                obj.Title = item.Title; 
                obj.Description = item.Description;
                obj.IsFavorite  = item.Isfavorite;
                obj.GradeId = item.GradeId;
                obj.Id= item.Id;  
                obj.CreateDate = item.CreateDate;
                obj.UpdateDate = item.UpdateDate;
                obj.AssignedDate= item.AssignedDate;    
                    
                res.Add(obj);   

            }
                       
           return res;  
        }
        public async Task<ActionMessageResponse> InsertOrUpdateAssignments(AssignmentsRequest assignmentsRequest) {
            if (assignmentsRequest == null) {
                return new ActionMessageResponse { Success = false };
            }

            Assignments assignments = new Assignments {
                Title = assignmentsRequest.Title,
                Description = assignmentsRequest.Description,
                Id = assignmentsRequest.Id,
                Teacherid = assignmentsRequest.TeacherId,
                Isdeleted = assignmentsRequest.IsDeleted,
                Subjectid = assignmentsRequest.SubjectId,
                GradeId = assignmentsRequest.GradeId,
                Isfavorite = assignmentsRequest.IsFavorite,
            };

            int id = assignments.Id == 0
                ? await _assignmentsRepo.InsertAssignment(assignments)
                : await _assignmentsRepo.UpdateAssignment(assignments);

            try {
                if (assignmentsRequest.UploadedFiles != null && assignmentsRequest.UploadedFiles.Any()) {
                    await ProcessUploadedFiles(id, assignmentsRequest.UploadedFiles);
                }
            } catch (Exception) {
                throw;
            }

            string message = assignments.Id == 0 ? "Assignment_created" : "Assignment_Updated";
            return new ActionMessageResponse { Content = id, Message = message, Success = true };
        }

        private async Task ProcessUploadedFiles(int entityId, List<FileUploadResponse> uploadedFiles) {
            _mediaFileRepository.DeleteMediaFIle(entityId, (int)Entities.DTOs.Enum.MediaEntityType.Assignment);
            foreach (var item in uploadedFiles) {
                MediaFileRequest mediaFile = new MediaFileRequest {
                    FileName = item.FileName,
                    Bloblink = item.FileLink,
                    EntityId = entityId,
                    EntityTypeId = Entities.DTOs.Enum.MediaEntityType.Assignment
                };
                _mediaFileRepository.InsertInMediaFile(mediaFile);
            }
        }

    }

}
