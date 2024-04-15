using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entites.DataModels {
    public  class UserDevices {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DeviceToken { get; set; }
        public DateTime CreatedDate { get; set; }
    
    }
}
