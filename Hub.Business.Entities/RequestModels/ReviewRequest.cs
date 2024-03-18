using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels
{
    public class ReviewRequest
    {
        public int UserId { get; set; }
        public int BatchId { get; set; }    
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
