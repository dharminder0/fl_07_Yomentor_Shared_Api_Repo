
namespace Core.Business.Entities.DTOs {
    public class BatchDto {
        public int Id { get; set; }
        public string Description { get; set; }
        public string BatchName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan TuitionTime { get; set; }
        public string Fee { get; set; }
        public string FeeType { get; set; }
        public int StudentCount { get; set; }
        public List<string> Days { get; set; }
        public int ActualStudents { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public TeacherInformation TeacherInformation { get; set; }


    }

    public class TeacherInformation {
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Phone { get; set; }
    }
}
