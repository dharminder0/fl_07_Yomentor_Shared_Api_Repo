using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.RequestModels {
    public class UserRequest {
        public int Id { get; set; }
        public string ? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Phone { get; set; }
        public string ? Email { get; set; }
        public int? Type { get; set; }
        public DateTime ? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime ? LastLoginDate { get; set; }
        public bool ? IsDeleted { get; set; }
        public int Parentid { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string ? Password { get; set; }
        public int ? Rank { get; set; }
        public int ? GradeId { get; set; }

       
    }
    public class AuthenticationRequest {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
