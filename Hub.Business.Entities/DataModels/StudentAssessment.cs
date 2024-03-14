using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Student_Assessment")]
    public class StudentAssessment {
        public StudentAssessment() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public int AssessmentId { get; set; }
        public string Status { get; set; }
        public double Marks { get; set; }
    }
}
