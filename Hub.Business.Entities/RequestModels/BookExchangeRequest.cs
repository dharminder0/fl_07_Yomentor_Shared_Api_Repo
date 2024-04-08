using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class BookExchangeRequest {
        public int UserId { get; set; }
        public int UserType { get; set; }
        public List<int>? StatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        
    }
}
