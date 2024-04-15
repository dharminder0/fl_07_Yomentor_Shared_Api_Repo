using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.RequestModels {
    public  class UserDevicesRequest {
        public int Id { get; set; }
        public List<int> UserId { get; set; }
        public string DeviceToken { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
