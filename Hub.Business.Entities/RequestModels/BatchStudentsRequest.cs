using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
   
    public class BatchStudentsRequest
    {

        public int Id { get; set; }
        public List<StudentAttendance> student_Info{ get; set; }
        public int BatchId { get; set; }
    
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
  
    public class student_Info {
        public int StudentId { get; set; }
        public int Status { get; set; }
    }
}
