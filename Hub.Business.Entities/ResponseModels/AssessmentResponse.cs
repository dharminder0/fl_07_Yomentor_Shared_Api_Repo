using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class AssessmentResponse {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public int GradeId { get; set; }
        public int Subjectid { get; set; }
        public int Maxmark { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public  string GradeName { get; set; }
        public string SubjectName { get; set; }
        public DateTime AssignedDate { get; set; }
        public List<FileUploadResponse> UploadFiles  { get; set; }
    }
}
