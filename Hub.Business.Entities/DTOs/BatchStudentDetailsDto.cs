using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DTOs
{
    public class BatchStudentDetailsDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address UserAddress { get; set; }
        public int BatchId { get; set; }    
        public int StudentId { get; set; }
        public string SubjectName { get; set; } 
        public string EnrollmentStatus { get; set; }
        public int enrollmentstatus {  get; set; }
        public string  Image { get; set; }
    }
}
