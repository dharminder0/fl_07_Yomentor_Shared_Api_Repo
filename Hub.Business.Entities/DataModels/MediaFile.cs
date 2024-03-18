using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class MediaFile {
        public int Id { get; set; }
        public int MediaTypeId { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string FileName { get; set; }
        public string BlobLink { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
