using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class SkillTestAttemptRequest {
        public int AttemptId { get; set; }
        public DateTime CreatedDate { get; set; }
        public  List<AttemptedQuestion> AttemptedQuestions { get; set; }
    }
    public class AttemptedQuestion {
        public int QuestionId { get; set;}
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
