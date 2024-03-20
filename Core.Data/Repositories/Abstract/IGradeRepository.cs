using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;

namespace Core.Data.Repositories.Abstract {
    public interface IGradeRepository : IDataRepository<Grade> {
        Task<IEnumerable<Grade>> GetAllGrades();
        string GetGradeName(int id);
    }
}
