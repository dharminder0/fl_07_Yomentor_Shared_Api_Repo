using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class StudentAssessmentRequest {
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public int AssessmentId { get; set; }
        public string Status { get; set; }
        public double Marks { get; set; }
    }
}
