using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class GradeResponse {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Isdeleted { get; set; } = false;
    }
}
