using Core.Common.Data;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Assignments")]
    public class Assignments {
        public Assignments() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public int Teacherid { get; set; }
        public string  Title { get; set; }
        public string  Description { get; set; }
        public int GradeId { get; set; }
        public int Subjectid { get; set; }
        public bool Isfavorite { get; set; }
        public bool Isdeleted { get; set; }
    }
}
