using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IStudentAssessmentRepository : IDataRepository<StudentAssessment>{
        Task<int> InsertStudentAssessment(StudentAssessment studentAssessment);
        Task<int> UpdateStudentAssessment(StudentAssessment studentAssessment);
        bool DeleteStudentAssessment(int batchId, int assessmentId);
    }
}
