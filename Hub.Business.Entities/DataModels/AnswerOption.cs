using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class AnswerOption {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public bool IsCorrect { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDeleted { get; set; }= false; 
    }
}
