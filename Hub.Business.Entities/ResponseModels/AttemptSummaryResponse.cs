using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class AttemptSummaryResponse {
        public int AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double PercentageCorrect { get; set; }
    }
    public class QuestionAnswerResponse {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class AttemptDetailsResponse {
        public int AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double PercentageCorrect { get; set; }
        public List<QuestionAnswerResponse> Questions { get; set; }
    }
}
