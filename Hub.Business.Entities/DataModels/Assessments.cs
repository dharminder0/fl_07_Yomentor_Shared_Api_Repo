using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Assessments")]
    public class Assessments {
        public Assessments() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string  Title { get; set; }
        public int  GradeId { get; set; }
        public int Subjectid { get; set; }
        public int Maxmark { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDeleted { get; set; }
        public string  Description { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
    }
}
