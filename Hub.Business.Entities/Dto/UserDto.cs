using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Business.Entities.Dto {
    public class UserDto {
       
            public int Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int Type { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public DateTime LastLoginDate { get; set; }
            public bool IsDeleted { get; set; }
            public int Parentid { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
          
            public string Token { get; set; }
            public string Address { get; set; }
            public string  About { get; set; }
            public string  Education { get; set; }
            public string  Experience { get; set; }
            public string  Image { get; set; }

    }
    public class UserAuthenticationDto : UserDto {
        public bool AuthenticationStatus { get; set; }
    }
}
