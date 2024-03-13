using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class AssignmentsService :IAssignmentsService{
        private readonly IAssignmentsRepository _assignmentsRepo;
        public AssignmentsService(IAssignmentsRepository assignmentsRepo)
        {
            _assignmentsRepo = assignmentsRepo;
                
        }
        public async Task<ActionMassegeResponse> InsertOrUpdateAssignments(AssignmentsRequest assignmentsRequest) {
            if(assignmentsRequest == null) {
                return new ActionMassegeResponse { Response = false };
            }
            Assignments assignments = new Assignments();
            assignments.Title = assignmentsRequest.Title;   
            assignments.Description = assignmentsRequest.Description;       
            assignments.Id = assignmentsRequest.Id;
            assignments.Teacherid = assignmentsRequest.Teacherid;   
            assignments.Isdeleted = assignmentsRequest.Isdeleted;
            assignments.Subjectid = assignmentsRequest.Subjectid;
            assignments.GradeId = assignments.GradeId;
            assignments.Isfavorite = assignmentsRequest.Isfavorite; 
            if (assignmentsRequest.Id == 0) {
              int InsertedId= await  _assignmentsRepo.InsertAssignment(assignments);
                return new ActionMassegeResponse { Content = InsertedId, Message = "Assignment_created", Response = true };
            }
            int id=await _assignmentsRepo.UpdateAssignment(assignments);   
            return new ActionMassegeResponse { Content = id, Message = "Assignment_Updated", Response = true };

        }
    }
}
