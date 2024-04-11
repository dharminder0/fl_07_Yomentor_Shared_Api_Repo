using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.DataModels {
    public  class PushNotifications {
        public int Id { get; set; }
        public int NotificationTypeId { get; set; }
        public int UserId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime NotificationDateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public bool IsRead { get; set; }
        public int NotificationStatus { get; set; }
    }
}
