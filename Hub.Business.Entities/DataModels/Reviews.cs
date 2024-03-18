using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    [Alias(Name = "Reviews")]
    public class Reviews
    {
        public Reviews() { }    
        public int Id { get; set; } 
        public int AddedBy { get; set; }
        public int AddedFor { get; set; }
        public int BatchId {  get; set; }   
        public int Rating { get; set; } 
        public string Review { get; set; }
        public DateTime CreateDate { get; set; }= DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public bool isDeleted { get; set; }

    }
}
