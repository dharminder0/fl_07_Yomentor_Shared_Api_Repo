using Core.Business.Entities.ChatGPT;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Abstract {
    public interface ISkillTestRepository : IDataRepository<SkillTest> {
        Task<IEnumerable<SkillTest>> GetSkillTestList(SkillTestRequest skillTest);
        SkillTest GetSkillTest(int Id);
        int GetSkillTestSumScore(int Id);
        int GetSkillTestUser(int Id);
        int UpsertAttempt(Attempt attempt);
        Task<IEnumerable<Question>> GetQuestions(int skillTestId);
        Task<IEnumerable<AnswerOption>> GetAnswerOptionsForQuestion(int questionId);
        IEnumerable<Attempt> GetAttemptHistory(int userId, int skilltestId);
        bool InsertAttemptDetail(AttemptDetail attemptDetail);
        bool DeleteAttemptDetail(int attemptId);
        int GetCorrectAnswer(int questionId);
        AttemptSummaryResponse CalculatePercentage(int attemptId);
        int UpdateScore(int attemptId, double score);
        int GetAnswerId(int attemptId, int questionId);
        IEnumerable<AnswerOption> GetAnswerList(int questionId);
        Task<int> InsertQuestion(Question question);
        Task<bool> InsertAnswerOption(AnswerOption answerOption);
        Task<int> InsertSkillTest(SkillTest skillTest);
        Prompt GetPrompt(int  categoryId);
        Prompt GetPromptByAcademicClass(int gradeid);
        Task<IEnumerable<SkillTest>> GetSkillTestListByUser(SkillTestRequest skillTest);
        IEnumerable<DailyAttemptCount> GetDailyAttemptCounts(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<SkillTest>> GetSimilerSkillTestList(SkillTestRequest skillTest);
       IEnumerable<Attempt> GetAttemptHistory(SkillTestRequest skillTest);
        IEnumerable<AttemptCount> GetAttemptCounts(int userId, SkillTestAttemptRange range);

        Task PromptLogs(string transactionId, string logLevel, string message, string stackTrace = null, string requestPayload = null, string responsePayload = null);
    }
}
