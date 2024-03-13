using Core.Business.Entities.DataModels;
using Core.Common.Data;

namespace Core.Data.Repositories.Abstract {
    public interface IAssessmentsRepository : IDataRepository<Assessments>{
        Task<int> InsertAssessments(Assessments assignment);
        Task<int> UpdateAssessments(Assessments assignment);
    }
}
