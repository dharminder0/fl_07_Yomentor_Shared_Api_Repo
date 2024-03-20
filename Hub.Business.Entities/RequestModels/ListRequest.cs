using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class ListRequest {
        public int BatchId { get; set; }
        public int StudentId { get; set; }
        public int PageSize { get; set; } = 1;
        public int PageIndex { get; set; } = 10;
    }

}
