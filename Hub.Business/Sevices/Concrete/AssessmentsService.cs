using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class AssessmentsService:IAssessmentsService {
        private readonly IAssessmentsRepository _assessmentsRepository;
        public AssessmentsService(IAssessmentsRepository assessmentsRepository)
        {
            _assessmentsRepository = assessmentsRepository;
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
        public async Task<List<Assessments>> GetAssessmentsAllList()
        {
            try
            {
                var res = await _assessmentsRepository.GetAssessmentsAllList();
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
