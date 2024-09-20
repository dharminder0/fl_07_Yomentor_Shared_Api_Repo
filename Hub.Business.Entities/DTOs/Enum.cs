using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DTOs {
    public class Enum {
        public enum Days {
            S = 0,
            M = 1,
            T = 2,
            W = 3,
            Th = 4,
            F = 5,
            Sa = 6
        }
        public enum UserType {
            Teacher = 1,
            Parent = 2,
            Student = 3
        }
        public enum BatchStatus {
            Open = 1,
            Active = 2,
            Close = 3,
            Aborted = 4
        }
        public enum FeeType {
            Hour = 0,
            Day = 1,
            Week = 2,
            Month = 3
        }
        public enum Enrollmentstatus {
            Pending = 0,
            Enrolled = 1,
            Accepted = 2,
            Rejected = 3,
            Withdrawn = 4
        }

        public enum AttendanceStatus {
            Present = 1,
            Absent = 0,
        }
        public enum MediaEntityType {
            None = 0,
            Assignment = 1,
            Assessment = 2,
            Users = 3,
            Book = 4
        }
        public enum MediaType {
            None = 0,
            Image = 1,
            Video = 2,
            Pdf = 4,


        }
        public enum FavouriteEntityType {
            None = 0,
            Batch = 1,
            Teacher = 2
        }
        public enum TaskStatus {
            Assign = 1,
            Complete = 2
        }
        public enum BookExchangeStatus {
            Requested = 1,
            Accepted = 2,
            Declined = 3,
            Delivered = 4,
            Cancelled = 5
        }

        public enum BookActionType {
            IsRequested = 1,
            IsCreated = 2

        }
        public enum NotificationStatus {
            NotSent = 1,
            Sent = 2
        }
        public enum Status {
            Pending = 1,
            Delivered = 2,
            Failed = 3,
            DeviceNotFound = 4,
        }
        public enum NotificationType {
            student_enrolled = 1,
            enrollment_status_update = 2,
            assignment_assigned = 3,
            assessment_assigned = 4

        }
        public enum QuizStatus {
            Pending = 0,
            Complete = 1, Failed = 2,

        }
        public enum Language {
            HINDI = 1,
            ENGLISH = 2,
            URDU = 3,
            PUNJABI = 4,
            SANSKRIT = 5,

        }
        public enum ComplexityLevel {
            Easy=1,
            Medium=2,
            Hard=3,
            Advanced=4
        }
        public enum Category {
            Academic = 1,
            Competitive_Exams = 2

        }

        public enum SkillTestAttemptRange {

            Weekly=1,
            Monthly=2,
            SixMonthly=3,
            Yearly=4
        }
        public enum PageTypeEnum {
            HomePage = 1,
            AboutPage = 2,
            ContactPage = 3
           
        }
    }
}
