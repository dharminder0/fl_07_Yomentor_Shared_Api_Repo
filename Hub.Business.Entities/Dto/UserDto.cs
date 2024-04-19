using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Business.Entities.Dto {
    public class UserDto {
       
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int Type { get; set; }
            public DateTime CreateDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public DateTime LastLoginDate { get; set; }
            public bool IsDeleted { get; set; }
            public int ParentId { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Gender { get; set; }
          
            public string Token { get; set; }
        
            public string  Image { get; set; }
            public int AverageRating { get; set; }
            public int ReviewCount { get; set; }
            public TeacherSpecialityResponse TeacherSpeciality { get; set; }
            public TeacherProfileResponse TeacherProfile { get; set; }
            public Address UserAddress { get; set; }
            public int StudentGradeId { get; set; }
            public int Rank { get; set; }



    }
    public class UserAuthenticationDto : UserDto {
        public bool AuthenticationStatus { get; set; }
    }
   public class UserBasic {
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Phone { get; set; }
        public string  Email { get; set; }
        public Address UserAddress { get; set; }
        public string  UserImage { get; set; }
      
    }
    public class UserBasicV2 {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Address UserAddress { get; set; }
        public string UserImage { get; set; }
        public string ReceiverStatus { get; set; }
        public int ReceiverStatusId { get; set; }
    }
}
