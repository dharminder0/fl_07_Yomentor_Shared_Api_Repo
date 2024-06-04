using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class SkillTestRepository : DataRepository<SkillTest>, ISkillTestRepository {
        public async Task<IEnumerable<SkillTest>> GetSkillTestList(SkillTestRequest skillTest) {
            var sql = @" select * from skilltest where 1=1 ";
            if (skillTest.SubjectId > 0) {
                sql += @" and subjectId=@SubjectId  ";
            }
            if (skillTest.GradeId > 0) {
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
            return await QueryAsync<SkillTest>(sql, skillTest);
        }
        public SkillTest GetSkillTest(int Id) {
            var sql = @" select * from skillTest where id=@Id and IsDeleted=0 ";
            return QueryFirst<SkillTest>(sql, new { Id });
        }
        public int GetSkillTestSumScore(int Id) {
            var sql = @" select Avg(score) from attempt  where skilltestId=@id and status=1 ";
            return ExecuteScalar<int>(sql, new { Id });
        }
        public int GetSkillTestUser(int Id) {
            var sql = @" select Count(userId) from attempt  where skilltestId=@id  and status=1 ";
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

            return ExecuteScalar<int>(sql, attempt);
        }
        public async Task<IEnumerable<Question>> GetQuestions(int skillTestId) {
            var sql = @"select * from Question where skilltestid=@skillTestId";
            return await QueryAsync<Question>(sql, new { skillTestId });
        }
        public async Task<IEnumerable<AnswerOption>> GetAnswerOptionsForQuestion(int questionId) {
            var sql = @"SELECT A.*, Q.Explanations 
FROM answer_option A 
JOIN Question Q 
ON A.QuestionId = Q.id where A.questionId=@questionId ";
            return await QueryAsync<AnswerOption>(sql, new { questionId });
        }

        public IEnumerable<Attempt> GetAttemptHistory(int userId, int skilltestId) {
            var sql = @"
 select * from Attempt where userid=@userId  and skilltestid=@skilltestId and status=1  

 ";
            return Query<Attempt>(sql, new { userId, skilltestId });


        }
        public bool InsertAttemptDetail(AttemptDetail attemptDetail) {

            string sql = @"
            INSERT INTO Attempt_Detail
            (
                AttemptId,
                QuestionId,
                AnswerId,
                IsCorrect,
                CreateDate
            )
            VALUES
            (
                @AttemptId,
                @QuestionId,
                @AnswerId,
                @IsCorrect,
                GetUtcDate()
            )";
            return ExecuteScalar<bool>(sql, attemptDetail);



        }
        public bool DeleteAttemptDetail(int attemptId) {
            var sql = @" delete from attempt_detail where AttemptId=@attemptId ";
            return ExecuteScalar<bool>(sql, new { attemptId });
        }
        public int GetCorrectAnswer(int questionId) {
            var sql = @"select id from answer_option where questionId=@questionId and iscorrect=1";
            return ExecuteScalar<int>(sql, new { questionId });

        }
        public AttemptSummaryResponse CalculatePercentage(int attemptId) {
            var sql = @"SELECT
    attemptId,
    COUNT(*) AS TotalQuestions,
    SUM(CASE WHEN IsCorrect = 1 THEN 1 ELSE 0 END) AS TotalCorrectAnswers,
    (SUM(CASE WHEN IsCorrect = 1 THEN 1 ELSE 0 END) * 100.0) / COUNT(*) AS PercentageCorrect
FROM
    attempt_detail
WHERE
    attemptId =@attemptId
GROUP BY
    attemptId;
";
            return QueryFirst<AttemptSummaryResponse>(sql, new { attemptId });


        }
        public int UpdateScore(int attemptId, double score) {
            var sql = @"update  Attempt set score=@score,status=1 where  id=@attemptId   ";
            return ExecuteScalar<int>(sql, new { attemptId, score });
        }
        public int GetAnswerId(int attemptId, int questionId) {
            var sql = @" select answerId from attempt_detail where attemptid=@attemptId and questionId=@questionId ";
            return ExecuteScalar<int>(sql, new { attemptId, questionId });

        }
        public IEnumerable<AnswerOption> GetAnswerList(int questionId) {
            var sql = @"select * from answer_option where questionId=@questionId ";
            return Query<AnswerOption>(sql, new { questionId });

        }
        public async Task<int> InsertQuestion(Question question) {
            string sql = @"
        INSERT INTO Question
        (
            Title,
            Description,
            SkillTestId,
          explanations,
            CreateDate
        )
        VALUES
        (
            @Title,
            @Description,
            @SkillTestId,
          @explanations,
            GetUtcDate()
        );
            SELECT SCOPE_IDENTITY(); ";

            return await ExecuteScalarAsync<int>(sql, question);
        }
        public async Task<bool> InsertAnswerOption(AnswerOption answerOption) {
            string sql = @"
        INSERT INTO Answer_Option
        (
            QuestionId,
            Title,
            IsCorrect,
            CreateDate,
            IsDeleted
        )
        VALUES
        (
            @QuestionId,
            @Title,
            @IsCorrect,
            GetUtcDate(),
            @IsDeleted
        )";

            return await ExecuteScalarAsync<bool>(sql, answerOption);
        }
        public async Task<int> InsertSkillTest(SkillTest skillTest) {
            string sql = @"
    INSERT INTO SkillTest
    (
   
        Title,
        Description,
        GradeId,
        SubjectId,
        CreateDate,
        UpdateDate,
        IsDeleted,
        Topic,
        Prompt_Type,
        Complexity_Level,
        NumberOf_Questions,
        CreatedBy
    )
    VALUES
    (

        @Title,
        @Description,
        @GradeId,
        @SubjectId,
        GetUtcDate(),
        @UpdateDate,
        @IsDeleted,
        @Topic,
        @Prompt_Type,
        @Complexity_Level,
        @NumberOf_Questions,
        @CreatedBy
    );
            SELECT SCOPE_IDENTITY(); ";
            return await ExecuteScalarAsync<int>(sql, skillTest);
        }
        public Prompt GetPrompt(string prompt_type) {
            var sql = @" select * from Prompt where prompt_type=@prompt_type ";
            return QueryFirst<Prompt>(sql, new { prompt_type });
        }
    }
}
