using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class SkillTest {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
