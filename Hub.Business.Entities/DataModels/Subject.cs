using Core.Common.Data;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Subject")]
    public class Subject {
        public Subject() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public string  Name { get; set; }
        public int GradeId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
