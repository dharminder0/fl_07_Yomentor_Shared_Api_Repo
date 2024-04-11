using Core.Business.Entites.DataModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Abstract {
    public interface IPushnotificationsRepository:IDataRepository<PushNotifications> {
        int AddPushNotifications(int NotificationTypeId, int UserId, string Notificationtitle, string NotificationMessage, DateTime NotificationDateTime, DateTime CreatedDate, int status, int notificationStatus, int EntityId, bool IsRead);
        Task<bool> UpdateStatus(int id, Status status, int notificationType);
        Task<bool> UpdateNotificationStatus(int id, NotificationStatus status);
    }
}
