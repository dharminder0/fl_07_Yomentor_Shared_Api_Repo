using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class AssignmentsResponse {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string  GradeName { get; set; }
        public string  SubjectName { get; set; }
        public DateTime AssignedDate { get; set; }
        public List<FileUploadResponse> UploadedFiles { get; set; }
        public int FIlesCount { get; set; }
    }

}
