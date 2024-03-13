using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public  class BatchDetailRequest {
        public int Id { get; set; }
        public int  GradeId { get; set; }
        public DateTime ClassTime { get; set; }
        public DateTime Date { get; set; }
        public string  Days { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int NumberOfStudents { get; set; }
        public decimal Fee { get; set; }
        public string FeeType { get; set; }
        public int  SubjectId { get; set; }
        public int TeacherId { get; set; }
    }
    public class BatchDetailRequestV2 {
        public int Id { get; set; }
        public int GradeId { get; set; }
        public DateTime ClassTime { get; set; }
        public DateTime Date { get; set; }
        public List<string> Days { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int NumberOfStudents { get; set; }
        public decimal Fee { get; set; }
        public string FeeType { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
    }
}
