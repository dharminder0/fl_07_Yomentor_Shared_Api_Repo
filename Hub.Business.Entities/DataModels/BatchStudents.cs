using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    [Alias(Name = "BatchStudents")]
    public class BatchStudents
    {
        public BatchStudents() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; } 
        public int StudentId { get; set;}
        public int BatchId { get; set; }
        public int Enrollmentstatus {  get; set; }  
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; } 
    }
}
