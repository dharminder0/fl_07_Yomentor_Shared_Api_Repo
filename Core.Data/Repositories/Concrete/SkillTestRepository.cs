using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class SkillTestRepository  : DataRepository<SkillTest>, ISkillTestRepository{
        public async Task<IEnumerable<SkillTest>> GetSkillTestList(SkillTestRequest skillTest) {
            var sql = @" select * from skilltest where 1=1 ";
            if(skillTest.SubjectId > 0) {
                sql += @" and subjectId=@SubjectId  ";
            }
            if(skillTest.GradeId > 0) {
                sql += @" and gradeId=@gradeId ";
            }
            if (!string.IsNullOrWhiteSpace(skillTest.SearchText)) {
                sql += $@"
        AND (title LIKE '%{skillTest.SearchText}%' OR          
             description LIKE '%{skillTest.SearchText}%')";
            }
            if (skillTest.PageIndex > 0 && skillTest.PageSize > 0) {
                sql += $@"
ORDER BY id DESC
    
        OFFSET (@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY;";
            }
            return  await QueryAsync<SkillTest>(sql, skillTest);
        }
        public  SkillTest GetSkillTest(int Id) {
            var sql = @" select * from skillTest where id=@Id and IsDeleted=0 ";
            return QueryFirst<SkillTest>(sql, new { Id });  
        }
        public int GetSkillTestSumScore(int Id) {
            var sql = @" select sum(score) from attempt  where skilltestId=@id";
            return ExecuteScalar<int>(sql,  new { Id });
        }
        public int GetSkillTestUser(int Id) {
            var sql = @" select userId from attempt  where skilltestId=@id";
            return ExecuteScalar<int>(sql, new { Id });
        }
    }
}
