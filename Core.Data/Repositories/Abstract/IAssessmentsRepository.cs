using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;

namespace Core.Data.Repositories.Abstract {
    public interface IAssessmentsRepository : IDataRepository<Assessments>{
        Task<int> InsertAssessments(Assessments assignment);
        Task<int> UpdateAssessments(Assessments assignment);
        Assessments GetAssessments(int id);
        Task<List<Assessments>> GetAssessmentsAllList(StudentProgressRequestV2 request);
        Task<IEnumerable<Assessments>> GetAssessmentsByBatch(ListRequest listRequest );
    }
}
