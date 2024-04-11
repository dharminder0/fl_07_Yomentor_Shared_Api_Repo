using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class BookExchangeRequest {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public List<int>? StatusId { get; set; }
        public int? PageSize { get; set; }
        public int? PageIndex { get; set; }
        
    }
    public class  BookRequestV2
    {
        public int UserId { get; set; }
        public bool IsRequested { get; set; }
        public bool IsCreated { get; set; }
        public int GradeId { get; set; }
        public string ?  SearchText { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }


    }
}
