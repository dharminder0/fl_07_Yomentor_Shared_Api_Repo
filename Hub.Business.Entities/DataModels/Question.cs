using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class Question {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SkillTestId { get; set; }
        public int CorrectOption { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
