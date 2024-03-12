using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;

namespace Core.Data.Repositories.Concrete {
    public class SubjectRepository : DataRepository<Subject> ,ISubjectRepository{
        public async Task<IEnumerable<Subject>> GetAllSubjects( int gradeId) {
            var sql = @" select * from Subject where gradeId=@gradeId ";
            return await QueryAsync<Subject>(sql,new { gradeId }); 

        }
    }
}
