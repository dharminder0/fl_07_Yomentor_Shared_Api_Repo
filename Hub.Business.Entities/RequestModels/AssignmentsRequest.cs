using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class AssignmentsRequest {
        public int Id { get; set; }
        public int Teacherid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int Subjectid { get; set; }
        public bool Isfavorite { get; set; } = true;
        public bool Isdeleted { get; set; }= false;
        public List<FileUploadResponse> uploadedFiles { get; set; }
    }
}
