using Core.Common.Data;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Conversation")]
    public class Conversation {
        public int ConversationId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }

    }
}
