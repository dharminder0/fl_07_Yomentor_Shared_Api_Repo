using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class AttemptSkillTestResponse {
        public int QuestionId { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public int SkillTestId { get; set; }
        public DateTime QuestionCreateDate { get; set; }
        public List<AnswerOption> AnswerOptions { get; set; }
    }
}
