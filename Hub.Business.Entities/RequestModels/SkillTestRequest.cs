using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class SkillTestRequest {
        public string ? SearchText { get; set; }        
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
