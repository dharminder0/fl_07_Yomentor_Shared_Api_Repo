using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class PromptLogs {
        public int Id { get; set; }
        public string  log_level { get; set; }
        public  string  log_message { get; set;}
        public string stack_trace { get; set; }
        public DateTime Timestamp { get; set; }
        public string TransactionId { get; set; }

    }
}
