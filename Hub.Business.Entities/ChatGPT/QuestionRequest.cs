using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ChatGPT {
    public class QuestionRequest {
        public string? UserId { get; set; }
        public int Category { get; set; }
        public int AcademicClass { get; set; }
        public int Subject { get; set; }
        public string? Topic { get; set; }
        public int ComplexityLevel { get; set; }
        public int NumberOfQuestions { get; set; }
      
        public int Language { get; set; }
    }

    public class QuestionResponse {
        public string Question { get; set; }
        public string correct_answer { get; set; }
        public string Explanation { get; set; }
        public List<string> Choices { get; set; }

    }
    public class Questionnaire {
        public int SkillTestId { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<QuestionResponse> Questions { get; set; }
    }


    public class ChatGPTRequest {
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public string Subject { get; set; }


    }
}
