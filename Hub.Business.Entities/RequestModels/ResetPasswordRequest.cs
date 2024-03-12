using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public  class ResetPasswordRequest {
        public string Phone { get; set; }
        public string NewPassword { get; set; }
        public string UserToken { get; set; }
    }
    public class ChangePasswordRequest {
        public string phone { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
