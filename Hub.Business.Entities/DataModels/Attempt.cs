using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class Attempt {
        public int Id { get; set; }
        public string AttemptCode { get; set; }
        public int UserId { get; set; }
        public int SkillTestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Status { get; set; }
        public int Score { get; set; }
    }
}
