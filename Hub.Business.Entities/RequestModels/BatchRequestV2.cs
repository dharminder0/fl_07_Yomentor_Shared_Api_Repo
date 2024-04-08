using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class BatchRequestV2 {
        public int teacherId { get; set; }
        public int StudentId { get; set; }
        public List<int>? StatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
    

    }
}
