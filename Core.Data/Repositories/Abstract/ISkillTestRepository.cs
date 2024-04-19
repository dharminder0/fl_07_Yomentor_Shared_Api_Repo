using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface ISkillTestRepository  : IDataRepository<SkillTest>{
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
    }
}
