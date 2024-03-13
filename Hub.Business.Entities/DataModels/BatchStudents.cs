using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    public class BatchStudents
    {
        public int Id { get; set; } 
        public int StudentId { get; set;}
        public int BatchId { get; set; }
        public int Enrollmentstatus {  get; set; }  
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; } 
    }
}
