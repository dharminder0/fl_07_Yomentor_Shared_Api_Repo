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
       List<AssessmentResponse> GetAssessmentsList(int id);
        Task<List<AssessmentResponse>> GetAssessmentsAllList(StudentProgressRequestV2 request);
        Task<ActionMassegeResponse> AssignStudentAssessment(StudentAssessmentRequestV2 requestV2);
        Task<List<AssessmentResponse>> GetAssessmentByBatch(ListRequest request);
        bool DeleteAssessment(int Id);
        ActionMassegeResponse DeleteStudentAssessments(int batchId, int assesmentid);
    }
}
