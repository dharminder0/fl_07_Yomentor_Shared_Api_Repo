using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class AssessmentsRequest {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public int Maxmark { get; set; }
        public bool Isfavorite { get; set; }=true;
        public bool Isdeleted { get; set; }=false;  
        public string Description { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<FileUploadResponse> uploadedFiles { get; set; }
    }
}
