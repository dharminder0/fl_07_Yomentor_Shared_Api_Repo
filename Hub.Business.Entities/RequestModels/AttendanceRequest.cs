using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class AttendanceRequest {
        public int BatchId { get; set; }
        public int StudentId { get; set; }
        public string  fromDate { get; set; }
        public string  ToDate { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
}
