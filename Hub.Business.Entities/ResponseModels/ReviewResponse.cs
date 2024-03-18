using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels
{
    public class ReviewResponse
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int AddedByUserId { get; set; }
        public string AddedByFirstName { get; set; }
        public string AddedByLastName { get; set; }
        public int BatchId { get; set; }
        public string BatchTitle { get; set; }
        public int Rating { get; set; }
        public string Review {  get; set; } 
        public DateTime CreateDate { get; set; }    
        public DateTime UpdateDate { get; set; }
    }
}
