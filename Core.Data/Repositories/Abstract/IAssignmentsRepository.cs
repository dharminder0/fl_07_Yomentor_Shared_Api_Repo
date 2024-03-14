using Core.Business.Entities.DataModels;
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
        Task<List<Assignments>> GetAllAssignments(int teacherid);
        Task<IEnumerable<Assignments>> GetAssignmentsByBatch(int batchId);

    }
}
