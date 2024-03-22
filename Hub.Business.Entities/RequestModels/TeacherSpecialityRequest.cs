using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class TeacherSpecialityRequest {
        public int TeacherId { get; set; }
        public List<GradeSubjectResponse> GradeSubjectList { get; set; }
    }
    public class GradeSubjectResponse {
        public int SubjectId { get; set; }
        public int GradeId { get; set; }
    }
}
