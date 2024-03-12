using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class GradeRepository : DataRepository<Grade>, IGradeRepository {
      public  async Task<IEnumerable<Grade>> GetAllGrades() {
            var sql = @"select * from Grade ";
            return  await  QueryAsync<Grade>(sql); 
        }
    }
}
