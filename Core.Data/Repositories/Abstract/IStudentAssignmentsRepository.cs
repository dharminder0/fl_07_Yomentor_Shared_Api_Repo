using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IStudentAssignmentsRepository:IDataRepository<Student_Assignments>
    {
        Task<int> InsertStudentAssignment(Student_Assignments studentAssignment);
        Task<int> UpdateStudentAssignment(Student_Assignments studentAssignment);
        Task<bool> DeleteStudentAssignment(int batchId);
        bool DeleteStudentAssignments(int batchId, int assignmentid);
    }
}
