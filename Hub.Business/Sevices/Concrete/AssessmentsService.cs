using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class AssessmentsService:IAssessmentsService {
        private readonly IAssessmentsRepository _assessmentsRepository;
        private readonly IBatchStudentsRepository _batchStudentsRepository;
        private readonly IStudentAssessmentRepository _studentAssessmentRepository; 
        public AssessmentsService(IAssessmentsRepository assessmentsRepository, IBatchStudentsRepository batchStudentsRepository, IStudentAssessmentRepository studentAssessmentRepository)
        {
            _assessmentsRepository = assessmentsRepository;
            _batchStudentsRepository= batchStudentsRepository; 
            _studentAssessmentRepository= studentAssessmentRepository;
        }
        public async Task<ActionMessageResponse> InsertOrUpdateAssessments(AssessmentsRequest assessmentsRequest) {
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
                return new ActionMessageResponse { Content = insertedId, Message = "Assignment_created", Success = true };
            }

            int id = await _assessmentsRepository.UpdateAssessments(assessments);
            return new ActionMessageResponse { Content = id, Message = "Assignment_Updated", Success = true };
        }

        public IEnumerable<Assessments> GetAssessmentsList(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("id is null or blank");
            }
            try
            {
                var res = _assessmentsRepository.GetAssessmentsList(id);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<Assessments>> GetAssessmentsAllList(StudentProgressRequest request)
        {
            try
            {
                var res = await _assessmentsRepository.GetAssessmentsAllList(request);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<ActionMassegeResponse> AssignStudentAssessment(StudentAssessmentRequest request) {
            if (request == null) {
                return new ActionMassegeResponse { Response = false };
            }

            var response = _batchStudentsRepository.GetBatchStudentsbybatchId(request.BatchId);
            if (response == null || !response.Any()) {
                return new ActionMassegeResponse { Message = "No students found for the given batch.", Response = false };
            }


            foreach (var item in response.Select(v => v.StudentId)) {
                StudentAssessment student = new StudentAssessment();
                student.Status = request.Status;
                student.AssessmentId = request.AssessmentId;
                student.BatchId = request.BatchId;
                student.StudentId = item;
                student.Marks= request.Marks;   
                var res = await _studentAssessmentRepository.InsertStudentAssessment(student);
            }


            return new ActionMassegeResponse { Message = "Assigned_Successfully", Response = true };
        }

        public async Task<List<AssessmentResponse>> GetAssessmentByBatch(ListRequest request) {
            if (request.BatchId == 0) {
                return new List<AssessmentResponse> { };
            }
            List<AssessmentResponse> res = new List<AssessmentResponse>();
            var response = await _assessmentsRepository.GetAssignmentsByBatch(request);
            foreach (var item in response) {
                AssessmentResponse obj = new AssessmentResponse();
                obj.Id = item.Id;
                obj.TeacherId = item.TeacherId;
                obj.Title = item.Title;
                obj.Description = item.Description;
                obj.GradeId = item.GradeId;
                obj.IsFavorite = item.IsFavorite;
                obj.Id = item.Id;
                obj.Createdate = item.Createdate;
                obj.Updatedate = item.Updatedate;
                res.Add(obj);

            }

            return res;
        }

    }
}
