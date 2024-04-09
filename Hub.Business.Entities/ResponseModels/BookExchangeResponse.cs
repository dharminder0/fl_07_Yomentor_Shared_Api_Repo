using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class BookExchangeResponse {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public int BookId { get; set; }
        public string  BookName { get; set; }
        public UserBasic UserInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }
        public string  StatusName { get; set; }
       
    }
}
