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
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            Wednesday = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }
        public enum UserType
        {
            Teacher=1,
            Parent=2,
            Student=3
        }
        public enum Status
        {
            Open=1,
            Active=2,
            Close=3,
            Aborted=4
        }
    }
}
