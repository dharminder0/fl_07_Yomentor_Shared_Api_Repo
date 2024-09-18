using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class SkillTest {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Topic { get; set; }
        public int Prompt_Type { get; set; }
        public int Complexity_Level { get; set; }
        public int NumberOf_Questions { get; set; }
        public int CreatedBy { get; set; }
        public int Language{ get; set; }
        public int TimerValue { get; set; }
        public bool isEnableTimer { get; set; }
    }
}
