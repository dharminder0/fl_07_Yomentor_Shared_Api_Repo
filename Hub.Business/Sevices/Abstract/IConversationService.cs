using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface IConversationService {
        ActionMassegeResponse UpsertConversation(Conversation conversation);
        ActionMassegeResponse UpsertMessage(Conversations_Messages message);
        Task<List<ConversationMessageResponse>> GetConversation(int conversationId);

    }
}
