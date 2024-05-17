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
    public interface IAssignmentsService {

        Task<ActionMessageResponse> InsertOrUpdateAssignments(AssignmentsRequest assignmentsRequest);
        Task<List<AssignmentsResponse>> GetAssignment(int id);
        Task<List<AssignmentsResponse>> GetAllAssignments(StudentProgressRequestV2 request);
        Task<ActionMassegeResponse> AssignStudentAssignments(StudentAssignmentsRequestV2 request);
        Task<List<AssignmentsResponse>> GetAssignmentsByBatch(ListRequest request);
        bool DeleteAssessment(int Id);
        ActionMassegeResponse RemoveStudentAssignments(int assesementId, int batchId);
    }
}
