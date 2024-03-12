using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    public class Batch
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; } 
        public int SubjectId {get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan TuitionTime { get; set; }
        public string Fee { get; set; }
        public int FeeType { get; set; }
        public int StudentCount { get; set; }
        public string Days { get; set; } 
       // public bool IsDeleted { get; set; }
        public int Status { get; set; }

    }
   
}
