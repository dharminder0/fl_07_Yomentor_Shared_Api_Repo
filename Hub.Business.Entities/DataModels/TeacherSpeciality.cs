using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "TeacherSpeciality")]
    public class TeacherSpeciality {
        public TeacherSpeciality() { }
        [Key(AutoNumber = true)]
   
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int GradeId { get; set; }
    }
}
