using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;

namespace Core.Business.Sevices.Concrete {
    public class AssignmentsService : IAssignmentsService {
        private readonly IAssignmentsRepository _assignmentsRepo;
        private readonly IStudentAssignmentsRepository _studentAssignmentsRepo;
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IGradeRepository _gradeRepository;
        public  readonly  ISubjectRepository _subjectRepository; 
        public AssignmentsService(IAssignmentsRepository assignmentsRepo, IStudentAssignmentsRepository studentAssignmentsRepo, IBatchRepository batchRepository, IBatchStudentsRepository batchStudentsRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository) {
            _assignmentsRepo = assignmentsRepo;
            _studentAssignmentsRepo = studentAssignmentsRepo;
            _batchRepository = batchRepository;
            _batchStudentsRepository = batchStudentsRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
        }
        public async Task<ActionMassegeResponse> InsertOrUpdateAssignments(AssignmentsRequest assignmentsRequest) {
            if (assignmentsRequest == null) {
                return new ActionMassegeResponse { Response = false };
            }
            Assignments assignments = new Assignments();
            assignments.Title = assignmentsRequest.Title;
            assignments.Description = assignmentsRequest.Description;
            assignments.Id = assignmentsRequest.Id;
            assignments.Teacherid = assignmentsRequest.Teacherid;
            assignments.Isdeleted = assignmentsRequest.Isdeleted;
            assignments.Subjectid = assignmentsRequest.Subjectid;
            assignments.GradeId = assignmentsRequest.GradeId;
            assignments.Isfavorite = assignmentsRequest.Isfavorite;
            if (assignmentsRequest.Id == 0) {
                int InsertedId = await _assignmentsRepo.InsertAssignment(assignments);
                return new ActionMassegeResponse { Content = InsertedId, Message = "Assignment_created", Response = true };
            }
            int id = await _assignmentsRepo.UpdateAssignment(assignments);
            return new ActionMassegeResponse { Content = id, Message = "Assignment_Updated", Response = true };
        }

        public IEnumerable<Assignments> GetAssignment(int id) {
            if (id <= 0) return Enumerable.Empty<Assignments>();
            try {
                var res = _assignmentsRepo.GetAssignments(id);
                return res;
            } catch (Exception ex) {
                return Enumerable.Empty<Assignments>();
            }

        }

        public async Task<List<AssignmentsResponse>> GetAllAssignments(StudentProgressRequestV2 request) {
            try {
                var response = await _assignmentsRepo.GetAllAssignments(request);
                List <AssignmentsResponse> res = new List<AssignmentsResponse>();  


                foreach (var item in response) {
                    AssignmentsResponse obj = new AssignmentsResponse();

                    obj.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                    obj.SubjectName = _subjectRepository.GetSubjectName(item.Subjectid);
                    obj.Subjectid = item.Subjectid;
                    obj.Teacherid = item.Teacherid;
                    obj.Title = item.Title;
                    obj.Description = item.Description;
                    obj.Isfavorite = item.Isfavorite;
                    obj.GradeId = item.GradeId;
                    obj.Id = item.Id;
                    obj.CreateDate = item.CreateDate;
                    obj.UpdateDate = item.UpdateDate;
                 
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
                obj.Subjectid = item.Subjectid; 
                obj.Teacherid=item.Teacherid;   
                obj.Title = item.Title; 
                obj.Description = item.Description;
                obj.Isfavorite  = item.Isfavorite;
                obj.GradeId = item.GradeId;
                obj.Id= item.Id;  
                obj.CreateDate = item.CreateDate;
                obj.UpdateDate = item.UpdateDate;
                obj.AssignedDate= item.AssignedDate;    
                    
                res.Add(obj);   

            }
                       
           return res;  
        }
    }

}
