using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class ActionMessageResponse {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Content { get; set; }
    }
}
