using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Entities.DataModels {
    public class Banners {
        public int Id { get; set; }
        public string UserType { get; set; }
        public int UserId { get; set; } 
        public string BannerUrl { get; set; }
        public PageTypeEnum PageType { get; set; }
    }
}
