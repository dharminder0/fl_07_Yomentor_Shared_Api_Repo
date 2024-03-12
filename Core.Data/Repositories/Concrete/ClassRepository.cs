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

        public Users GetTeacherById(int id) {
            var Sql = $@"Select * from dbo.Users where Id=@id and Type='1'";
            var res = Query<Users>(Sql, new { id });
            return (Users)res;
        }
      
    }
}
