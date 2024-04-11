using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DTOs
{
    public class Enum
    {
        public enum Days
        {
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
        public enum FeeType
        {
            Hour=0,
            Day = 1,
            Week = 2,
            Month = 3
        }
        public enum Enrollmentstatus
        {
             Pending=0,
             Enrolled=1,
             Accepted=2,
             Rejected=3,
             Withdrawn=4
        }

        public enum AttendanceStatus
        {
            Present=1,
            Absent=0,
        }
        public enum MediaEntityType {
            None = 0,
            Assignment=1,
            Assessment=2,
            Users=3,
            Book=4
        }
        public enum MediaType {
            None=0,
            Image=1,
            Video=2,
            Pdf=4,


        }
        public enum FavouriteEntityType {
            None=0,
            Batch=1,
            Teacher=2
        }
        public enum TaskStatus {
            Assign=1,
            Complete=2
        }
        public enum BookExchangeStatus {
            Pending=1,
            Requested=2,
            Accepted=3,
            Declined=4,
            Completed=5,
            Cancelled=6,
            
        }

    }
}
