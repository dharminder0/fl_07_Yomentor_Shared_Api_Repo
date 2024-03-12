
namespace Core.Business.Entities.DTOs
{
    public class BatchDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan TuitionTime { get; set; }
        public string Rate { get; set; }
        public int RateType { get; set; }
        public int StudentCount { get; set; }
        public string Days { get; set; }
        public bool IsDeleted { get; set; }
    }

    public enum Days
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
}
