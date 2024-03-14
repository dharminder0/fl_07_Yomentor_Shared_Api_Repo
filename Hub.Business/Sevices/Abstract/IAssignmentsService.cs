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
        Task<ActionMassegeResponse> InsertOrUpdateAssignments(AssignmentsRequest assignmentsRequest);
        IEnumerable<Assignments> GetAssignment(int id);
        Task<List<Assignments>> GetAllAssignments(int teacherid);
        Task<ActionMassegeResponse> AssignStudentAssignments(StudentAssignmentsRequest request);
        Task<List<AssignmentsResponse>> GetAssignmentsByBatch(int batchId);
    }
}
