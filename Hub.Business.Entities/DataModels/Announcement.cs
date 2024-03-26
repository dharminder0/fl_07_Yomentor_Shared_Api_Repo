using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    [Alias(Name = "Announcements")]
    public class Announcements
    {
        public Announcements(){}
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int BatchId { get; set; }
        public string Announcement { get;set; }
        public DateTime CreateOn { get; set; }= DateTime.Now;
        public DateTime UpdateOn { get; set; }  

    }
}
