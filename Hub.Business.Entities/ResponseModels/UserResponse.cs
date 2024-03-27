using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class UserResponse {
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Phone { get; set; }
        public int AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string  ProfilePicture { get; set; }
        public string About { get; set; }
        public string Education { get; set; }
        public string Experience { get; set; }
    }
}
