using Core.Common.Data;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Users")]
    public class Users {
        public Users() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName  { get; set; }
        public string  Phone { get; set;}
        public string  Email { get; set;}
        public int Type { get; set; }
        public DateTime CreateDate { get; set;}
        public DateTime UpdateDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Parentid { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Token { get; set; }
     
        public int Rank { get; set; }
        public int GradeId { get; set; }
        public int Category { get; set; }
    }
}
