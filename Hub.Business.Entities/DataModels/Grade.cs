using Core.Common.Data;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Grade")]
    public class Grade {
        public Grade() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Isdeleted { get; set; }
    }
}
