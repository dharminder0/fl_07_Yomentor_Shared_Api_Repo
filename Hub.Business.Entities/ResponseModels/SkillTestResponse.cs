using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class SkillTestResponse {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string GradeName { get; set; }
        public string SubjectName { get; set; }
        public bool IsDeleted { get; set; }
        public int AverageMarks { get; set; }
        public int AttemptCount { get; set; }
        public List<AttemptHistory> AttemptHistory { get; set; }  
    }
    public class AttemptHistory {
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }
        public int AttemptId { get; set; }

    }
    public class ProcessedResponseV2 {
        public string Question { get; set; }
        public List<string> Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }

    public class ProcessedResponse {
        public List<QuestionInfo> Questions { get; set; }
    }

    public class QuestionInfo {
        public string Title { get; set; }
        public string Description { get; set; }
        public int SkillTestId { get; set; }
        public string CorrectOption { get; set; }
        public List<AnswerInfo> AnswerOptions { get; set; }
    }

    public class AnswerInfo {
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
    }


    public class ExamResponse {
        public bool Item1 { get; set; }
        public List<Questions> Item2 { get; set; }
    }

    public class Questions {
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public List<string> Choices { get; set; }
    }

    public class Choice {
        public int Index { get; set; }
        public Message Message { get; set; }
        public object Logprobs { get; set; }
        public string FinishReason { get; set; }
    }

    public class Message {
        public string Role { get; set; }
        public string Content { get; set; }
    }

    public class Usage {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }


}
