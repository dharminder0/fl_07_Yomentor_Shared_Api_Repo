using Core.Business.Entities.DataModels;
using Core.Common.Data;

namespace Core.Data.Repositories.Abstract {
    public interface ITeacherSpecialityRepository : IDataRepository<TeacherSpeciality> {
        Task<int> InsertTeacherSpeciality(TeacherSpeciality teacherSpeciality);
        Task<int> DeleteTeacherSpeciality(int teacherId);
    }
}
