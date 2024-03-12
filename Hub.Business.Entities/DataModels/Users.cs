using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class Users {
        public int id { get; set; }
        public string  firstname { get; set; }
        public string lastname { get; set; }    
        public string  phone { get; set;}
        public string  email { get; set;} 
        public DateTime createdate { get; set;}
        public DateTime updatedate { get; set; }
        public DateTime lastlogindate { get; set; }
        public bool isdeleted { get; set; }
        public int parentid { get; set; }
    }
}
