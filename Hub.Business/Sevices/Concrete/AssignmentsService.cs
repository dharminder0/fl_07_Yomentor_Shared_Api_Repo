using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Slapper.AutoMapper;

namespace Core.Business.Sevices.Concrete {
    public class AssignmentsService : IAssignmentsService {
        private readonly IAssignmentsRepository _assignmentsRepo;
        private readonly IStudentAssignmentsRepository _studentAssignmentsRepo;
        private readonly IBatchRepository _batchRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        public AssignmentsService(IAssignmentsRepository assignmentsRepo, IStudentAssignmentsRepository studentAssignmentsRepo, IBatchRepository batchRepository, IBatchStudentsRepository batchStudentsRepository) {
            _assignmentsRepo = assignmentsRepo;
            _studentAssignmentsRepo = studentAssignmentsRepo;
            _batchRepository = batchRepository;
            _batchStudentsRepository = batchStudentsRepository;

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

        public async Task<List<Assignments>> GetAllAssignments(StudentProgressRequest request) {
            try {
                var res = await _assignmentsRepo.GetAllAssignments(request);
                return res;
            } catch (Exception ex) {
                return null;
            }
        }
        public async Task<ActionMassegeResponse> AssignStudentAssignments(StudentAssignmentsRequest request) {
            if (request == null) {
                return new ActionMassegeResponse { Response = false };
            }

            var response = _batchStudentsRepository.GetBatchStudentsbybatchId(request.BatchId);
            if (response == null || !response.Any()) {
                return new ActionMassegeResponse { Message = "No students found for the given batch.", Response = false };
            }


            foreach (var item in response.Select(v => v.StudentId)) {
                Student_Assignments student = new Student_Assignments();
                student.Status = request.Status; 
                student.AssignmentId = request.AssignmentId;
                student.BatchId = request.BatchId;
                student.StudentId = item;
                var res = await _studentAssignmentsRepo.InsertStudentAssignment(student);
            }

            
            return new ActionMassegeResponse {  Message = "Assigned_Successfully", Response = true };
        }

       public  async Task<List<AssignmentsResponse>> GetAssignmentsByBatch(ListRequest request) {
            if (request.BatchId == 0) {
                return new List<AssignmentsResponse> { };
            }
            List<AssignmentsResponse> res = new List<AssignmentsResponse>();
          var response=await   _assignmentsRepo.GetAssignmentsByBatch(request);
            foreach (var item in response) {
                AssignmentsResponse obj = new AssignmentsResponse();
                obj.Subjectid = item.Subjectid; 
                obj.Teacherid=item.Teacherid;   
                obj.Title = item.Title; 
                obj.Description = item.Description;
                obj.Isfavorite  = item.Isfavorite;
                obj.GradeId = item.GradeId;
                obj.Id= item.Id;  
                obj.CreateDate= item.CreateDate;    
                obj.UpdateDate= item.UpdateDate;    
                res.Add(obj);   

            }
                       
           return res;  
        }
    }

}
