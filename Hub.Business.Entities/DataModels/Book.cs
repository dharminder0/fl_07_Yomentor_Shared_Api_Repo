using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Book")]
    public class Books {
        public Books() { }
        [Key(AutoNumber = true)]

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int PublicationYear { get; set; }
        public int GradeId { get; set; }
        public bool Available { get; set; }
 
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; }=false;
        public string  Status { get; set; }
        public string  Remark { get; set; }
        public int SubjectId { get; set; }

    }
}
