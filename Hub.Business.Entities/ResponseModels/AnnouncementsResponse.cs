using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels
{
   public class AnnouncementsResponse
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int BatchId { get; set; }
        public string Announcement { get; set; }
        public DateTime CreateOn { get; set; } 
        public DateTime UpdateOn { get; set; }
    }
}
