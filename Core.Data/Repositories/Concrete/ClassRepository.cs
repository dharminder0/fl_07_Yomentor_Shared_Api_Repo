using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class ClassRepository : DataRepository<Users>, IClassRepository {

        public Users GetUsers() {

            var sql = @"select * from Users ";
            return QueryFirst<Users>(sql);    
        }
    }
}
