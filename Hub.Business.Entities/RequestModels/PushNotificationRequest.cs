using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.RequestModels {
    public  class PushNotificationRequest {
        public int ID { get; set; }
        public int NotificationTypeId { get; set; }
        public int UserId { get; set; }
        public string Notificationtitle { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime NotificationDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BrandId { get; set; }
        public string Status { get; set; }
    }
}
