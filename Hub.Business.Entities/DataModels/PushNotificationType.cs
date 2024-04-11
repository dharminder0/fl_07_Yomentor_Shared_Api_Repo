using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.DataModels {
    public  class PushNotificationType {
        public int Id { get; set; }
        public string  Type { get; set; }
        public string  Title { get; set; }
        public string  Message { get; set; }
        public int BrandId { get; set; }
        public DateTime  CreatedDate { get; set; }
    }
}
