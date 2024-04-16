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
        public int UpsertAttempt(Attempt attempt) {
            var sql = @"
        IF NOT EXISTS (SELECT 1 FROM Attempt WHERE Id = @Id)
        BEGIN
            INSERT INTO Attempt
            (
                AttemptCode,
                UserId,
                SkillTestId,
                StartDate,
                CompleteDate,
                Status,
                Score
            )
            VALUES
            (
                @AttemptCode,
                @UserId,
                @SkillTestId,
                GetUtcDate(),
                @CompleteDate,
                @Status,
                @Score
            );

            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE Attempt
            SET
                AttemptCode = @AttemptCode,
                UserId = @UserId,
                SkillTestId = @SkillTestId,
                CompleteDate = @CompleteDate,
                Status = @Status,
                Score = @Score
            WHERE
                Id = @Id;

            SELECT Id FROM Attempt WHERE Id = @Id;
        END;
    ";

            return  ExecuteScalar<int>(sql, attempt);
        }
        public async Task<IEnumerable<Question>> GetQuestions(int skillTestId) {
            var sql = @"select * from Question where skilltestid=@skillTestId";
            return await QueryAsync<Question>(sql, new { skillTestId });
        }
        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsForQuestion(int questionId) {
            var sql = @"select * from answer_option where questionId=@questionId ";
            return  await QueryAsync<AnswerOption>(sql, new { questionId });
        }

    }
}
