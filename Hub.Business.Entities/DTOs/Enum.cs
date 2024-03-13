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
        public enum Status {
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
    }
}
