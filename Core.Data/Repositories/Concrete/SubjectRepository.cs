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
            var res = QueryFirst<string>(sql, new { subjectId });
            return res;
          
        }
        public string GetIcon(int  subjectId) {
            var sql = $"select icon from dbo.Subject where id=@subjectId";
            var res = QueryFirst<string>(sql, new { subjectId });
            return res;

        }
        public Subject GetSubjectDetails(int subjectId) {
            var sql = $"select *  from dbo.Subject where id=@subjectId";
            var res = QueryFirst<Subject>(sql, new { subjectId });
            return res;

        }


    }
}
