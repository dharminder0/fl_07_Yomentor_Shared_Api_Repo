﻿using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{

    public class AttendanceResponse
    {
  
        public int Id { get; set; } 
        public int StudentId { get; set;}
        public int BatchId { get; set; }
        public DateTime Date { get; set; }
        public int Status {  get; set; }    
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public DateTime UpdateDate { get; set; }
        public string   FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Phone { get; set; }
        public string  Image { get; set; }
        
    }
    public class AttendanceHistoryResponse {

        public int Id { get; set; }
        public int StudentId { get; set; }
        public int BatchId { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
      

    }
}
