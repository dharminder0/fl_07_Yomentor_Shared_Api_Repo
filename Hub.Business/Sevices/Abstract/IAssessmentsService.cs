using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface IAssessmentsService {
        Task<ActionMessageResponse> InsertOrUpdateAssessments(AssessmentsRequest assessmentsRequest);
        IEnumerable<Assessments> GetAssessmentsList(int id);
        Task<List<Assessments>> GetAssessmentsAllList(StudentProgressRequest request);
        Task<ActionMassegeResponse> AssignStudentAssessment(StudentAssessmentRequest request);
        Task<List<AssessmentResponse>> GetAssessmentByBatch(ListRequest request);
    }
}
