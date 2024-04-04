using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class ConversationMessageResponse {
     
            public int MessageId { get; set; }
            public int ConversationId { get; set; }
            public int SenderId { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        
    }
}
