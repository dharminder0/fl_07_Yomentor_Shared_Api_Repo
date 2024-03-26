using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class FavouriteBatchRequest {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsFavourite { get; set; }
        public DateTime CreatedDate { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityType { get; set; }
        
    }
}
