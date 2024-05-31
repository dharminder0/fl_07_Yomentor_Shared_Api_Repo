using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ChatGPT {
    public class QuestionRequest {
        public string ? UserId { get; set; }
        public string? Category { get; set; }
        public string ? AcademicClass { get; set; }
        public string? Subject { get; set; }
        public string? Topic { get; set; }
        public string? ComplexityLevel { get; set; }
        public int NumberOfQuestions { get; set; }
        public string? ExamName { get; set; }
    }

    public class QuestionResponse {
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public List<string> Choices { get; set; }
    }
    public class ChatGPTRequest {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Subject { get; set; }


    }
}
