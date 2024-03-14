using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Student_Assignments")]
    public class Student_Assignments {      
         public Student_Assignments() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public int AssignmentId { get; set; }
        public int Status { get; set; }
    }
}
