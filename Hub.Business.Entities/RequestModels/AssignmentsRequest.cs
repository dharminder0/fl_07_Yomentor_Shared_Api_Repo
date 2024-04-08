using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class AssignmentsRequest {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public bool IsFavorite { get; set; } = true;
        public bool IsDeleted { get; set; }= false;
        public List<FileUploadResponse> UploadedFiles { get; set; }
    }
}
