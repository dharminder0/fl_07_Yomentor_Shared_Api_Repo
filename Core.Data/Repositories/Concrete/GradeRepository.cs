using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Microsoft.Extensions.ObjectPool;
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

        public string GetGradeName(int id)
        {
            var sql = "SELECT name FROM Grade WHERE Id=@id";
            var res = Query<string>(sql, new { id });
            return res.FirstOrDefault(); 
        }
    }
}
