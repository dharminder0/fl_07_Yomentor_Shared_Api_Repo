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
      public  async Task<IEnumerable<Grade>> GetAllGrades(int type) {
            var sql = @"select * from Grade ";
            if(type > 0) {
                sql += @"where type=@type";
            }
            return  await  QueryAsync<Grade>(sql,new {type}); 
        }

        public string GetGradeName(int id)
        {
            var sql = "SELECT name FROM Grade WHERE Id=@id";
            var res = Query<string>(sql, new { id });
            return res.FirstOrDefault(); 
        }
        public int  GetGradeId(string  gradeName) {
            var sql = "SELECT id FROM Grade WHERE name=@gradeName ";
            var res = Query<int>(sql, new { gradeName });
            return res.FirstOrDefault();
        }
        public int GetCategory(int gradeId) {
            var sql = "SELECT type FROM Grade WHERE id=@gradeId ";
            var res = Query<int>(sql, new { gradeId });
            return res.FirstOrDefault();
        }
    }
}
