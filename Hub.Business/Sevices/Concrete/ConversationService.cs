using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class ConversationService : IConversationService {
        private readonly IConversationRepository _repo;
        public ConversationService(IConversationRepository repo) {
            _repo = repo;
        }
        public ActionMassegeResponse UpsertConversation(Conversation conversation) {
            if (conversation == null) {
                return new ActionMassegeResponse { Content = null, Response = false };
            }
            bool response = _repo.UpsertConversation(conversation);
            return new ActionMassegeResponse { Content = response, Response = true };
        }
        public ActionMassegeResponse UpsertMessage(Conversations_Messages message) {
            if (message == null) {
                return new ActionMassegeResponse { Content = null, Response = false };
            }
            bool response = _repo.UpsertMessage(message);
            return new ActionMassegeResponse { Content = response, Response = true };
        }
    }
}
