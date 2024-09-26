using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;

namespace Core.Data.Repositories.Abstract {
    public interface IGradeRepository : IDataRepository<Grade> {
        Task<IEnumerable<Grade>> GetAllGrades(int type);
        string GetGradeName(int id);
        int GetGradeId(string gradeName);
        IEnumerable<Category> GetCategories();
        string GetCategorieName(int id);
        Category GetCategorie(int id);


    }
}
