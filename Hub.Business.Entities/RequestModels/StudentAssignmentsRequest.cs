using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class StudentAssignmentsRequest {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public int AssignmentId { get; set; }
        public int Status { get; set; } = 0;

    }
}
