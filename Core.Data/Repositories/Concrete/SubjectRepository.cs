using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using static Slapper.AutoMapper;

namespace Core.Data.Repositories.Concrete {
    public class SubjectRepository : DataRepository<Subject> ,ISubjectRepository{
        public async Task<IEnumerable<Subject>> GetAllSubjects( int gradeId) {
            var sql = @" select * from Subject where gradeId=@gradeId ";
            return await QueryAsync<Subject>(sql,new { gradeId }); 

        }

        public string GetSubjectName(int subjectId)
        {
            var sql = $"select name from dbo.Subject where Id=@subjectId";
            var res = Query<string>(sql, new { subjectId });
            return res.FirstOrDefault(); 
        }
    }
}
