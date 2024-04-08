using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IConversationRepository:IDataRepository<Conversation> {
        bool UpsertConversation(Conversation conversation);
        bool UpsertMessage(Conversations_Messages message);
        Task<IEnumerable<Conversations_Messages>> GetConversation(int conversationId);

    }
}
