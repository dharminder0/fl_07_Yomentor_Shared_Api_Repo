using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IAssignmentsRepository : IDataRepository<Assignments> {
        Task<int> InsertAssignment(Assignments assignment);
        Task<int> UpdateAssignment(Assignments assignment);
        IEnumerable<Assignments> GetAssignments(int id);
        Task<List<Assignments>> GetAllAssignments(StudentProgressRequest request);
        Task<IEnumerable<Assignments>> GetAssignmentsByBatch(ListRequest request);

    }
}
