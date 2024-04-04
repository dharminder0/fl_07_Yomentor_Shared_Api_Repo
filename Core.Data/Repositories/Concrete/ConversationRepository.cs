using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class ConversationRepository :DataRepository<Conversation>, IConversationRepository{
        public bool UpsertConversation(Conversation conversation) {
            var sql = @"
        IF NOT EXISTS(SELECT 1 FROM Conversations WHERE ConversationId = @ConversationId)
        BEGIN
            INSERT INTO Conversations ( TeacherId, StudentId)
            VALUES ( @TeacherId, @StudentId)
        END
        ELSE
        BEGIN
            UPDATE Conversations
            SET TeacherId = @TeacherId, StudentId = @StudentId
            WHERE ConversationId = @ConversationId
        END
    ";

            return Execute(sql, new {
                ConversationId = conversation.ConversationId,
                TeacherId = conversation.TeacherId,
                StudentId = conversation.StudentId
            }) > 0;
        }
        public bool UpsertMessage(Conversations_Messages message) {
            var sql = @"
        IF NOT EXISTS(SELECT 1 FROM Conversations_Messages WHERE MessageId = @MessageId)
        BEGIN
            INSERT INTO Conversations_Messages ( ConversationId, SenderId, Content, Timestamp)
            VALUES ( @ConversationId, @SenderId, @Content, @Timestamp)
        END
        ELSE
        BEGIN
            UPDATE Conversations_Messages
            SET ConversationId = @ConversationId, SenderId = @SenderId, Content = @Content, Timestamp = @Timestamp
            WHERE MessageId = @MessageId
        END
    ";

            return Execute(sql, new {
                MessageId = message.MessageId,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId,
                Content = message.Content,
                Timestamp = message.TimeStamp
            }) > 0;
        }

        
    }
}
