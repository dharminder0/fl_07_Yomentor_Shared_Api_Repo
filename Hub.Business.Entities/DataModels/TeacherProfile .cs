using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "teacher_profile")]
    public class TeacherProfile {
        public TeacherProfile() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string  About { get; set; }
        public string  Education { get; set; }
        public string  Experience { get; set; }
    }
}
