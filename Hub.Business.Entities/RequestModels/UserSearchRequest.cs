using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class UserSearchRequest {
        public string?  SearchText { get; set; }
        public int? userType { get; set; }
        public string? grade { get; set; }
        public List<int>? subject { get; set; }
        public int PageSize { get; set; }
        public int pageIndex { get; set; }

    }
}
