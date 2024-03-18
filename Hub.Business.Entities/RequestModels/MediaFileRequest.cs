using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Entities.RequestModels {
    public class MediaFileRequest {
        public MediaEntityType EntityTypeId { get; set; }
        public MediaType MediaTypeId { get; set; } = MediaType.Image;
        public int EntityId { get; set; }
        public string FileName { get; set; }
        public string Bloblink { get; set; }
    
    }
    public class DeleteMediaFileRequest {
        public MediaEntityType EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string Bloblink { get; set; }

    }
}

