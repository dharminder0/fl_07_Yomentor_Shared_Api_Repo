using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class BookExchange {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int  ReceiverId { get; set; }
        public int  BookId { get; set; }
        public DateTime CreateDate { get; set; }
        public int  Status { get; set; }
    }
}
