﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class AddressRequest {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ? Address1 { get; set; }
        public string ? Address2 { get; set; }
        public string City { get; set; }
        public int  StateId { get; set; }
        public string Pincode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime  CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool  IsDeleted { get; set; }=false;
    }
}
