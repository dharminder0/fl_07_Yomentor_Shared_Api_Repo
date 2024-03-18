using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels
{
    [Alias(Name = "Attendance")]
    public class Attendance
    {
        public Attendance() { } 
        public int Id { get; set; } 
        public int StudentId { get; set;}
        public int BatchId { get; set; }
        public DateTime Date { get; set; }
        public int Status {  get; set; }    
        public DateTime CreateDate { get; set; }=DateTime.Now;
        public DateTime UpdateDate { get; set; }   
    }

    public class AttendanceV2 
    {
        public int Id { get; set; }
        public List<StudentAttendance> student_attendance { get; set; }
        public int BatchId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; }
    }
    public class StudentAttendance
    {
        public int StudentId { get; set; }
        public int Status { get; set; }
    }
}
