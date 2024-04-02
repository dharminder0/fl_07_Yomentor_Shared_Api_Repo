using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class TeacherProfileRequest {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
    }
}
