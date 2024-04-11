using Core.Business.Entites.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Concrete {
    public class PushnotificationsRepository :DataRepository<PushNotifications>,IPushnotificationsRepository {
        public int AddPushNotifications(int NotificationTypeId, int UserId, string Notificationtitle, string NotificationMessage, DateTime NotificationDateTime, DateTime CreatedDate, int status, int notificationStatus, int EntityId, bool IsRead) {

            var sql = @" INSERT INTO push_notifications
           (
                Notification_TypeId
                ,UserId
                ,Notification_title
                ,Notification_Message
                ,Notification_DateTime
                ,CreatedDate
                ,Status
                ,Notification_Status
                ,EntityId
                ,isRead
            )
     VALUES
           (
             @NotificationTypeId
                ,@UserId
                ,@Notificationtitle
                ,@NotificationMessage
                ,@NotificationDateTime
                ,GetUtcDate()
                ,@Status
                ,@notificationStatus
                ,@EntityId
                ,@isRead
            );
select SCOPE_IDENTITY() 
";
            return ExecuteScalar<int>(sql, new { NotificationTypeId, UserId, Notificationtitle, NotificationMessage, NotificationDateTime, CreatedDate, status, notificationStatus, EntityId, IsRead });


        }
        public async Task<bool> UpdateStatus(int id, Status status, int notificationType) {

            var sql = @" Update PushNotifications set status=@status  where Id=@id ";
            return await ExecuteScalarAsync<bool>(sql, new { id, status, notificationType });

        }
        public async Task<bool> UpdateNotificationStatus(int id, NotificationStatus status) {

            var sql = @" Update PushNotifications set NotificationStatus=@status where Id=@id ";
            return await ExecuteScalarAsync<bool>(sql, new { id, status });

        }
    }
}
