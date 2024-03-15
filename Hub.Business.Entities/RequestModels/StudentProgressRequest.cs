using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class StudentProgressRequest {
        public int TeacherId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

    }
}
