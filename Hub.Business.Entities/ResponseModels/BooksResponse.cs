using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class BooksResponse {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int PublicationYear { get; set; }
        public int GradeId { get; set; }
        public bool Available { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public UserBasic UserInfo { get; set; }
        public int  Status { get; set; }
        public string  StatusName { get; set; }
        public string  Remark { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string  GradeName { get; set; }
        public DateTime RequestedDate { get; set; }
        public List<UserBasic> ReceiverUsers { get; set; }
    }
    public class BookResponseV2 {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public int GradeId { get; set; }
        public string  GradeName { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string  ImageUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public int SubjectId { get; set; }
        public string  SubjectName { get; set; }
        public bool Available { get; set; }
        public DateTime RequestedDate { get; set; }


    }
}
