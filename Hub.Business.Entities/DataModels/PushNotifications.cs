using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.DataModels {
    public  class PushNotifications {
        public int ID { get; set; }
        public int NotificationTypeId { get; set; }
        public int UserId { get; set; }
        public string  Notificationtitle { get; set; }
        public string  NotificationMessage { get; set; }
        public DateTime NotificationDateTime { get; set; }
        public DateTime  CreatedDate { get; set; }
        public string  Status { get; set; }
        public int EntityId { get; set; }

        public bool IsRead { get; set; } = true;
      
        public int VisitType { get; set; }
        public int PatientUserId { get; set; }
        public int NotificationStatus { get; set; }
    }
}
