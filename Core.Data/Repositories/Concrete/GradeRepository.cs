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
            var sql = @"select * from Grade where 1=1  and isdeleted=0 ";
            if(type > 0) {
                sql += @"and  type=@type";
            }
            return  await  QueryAsync<Grade>(sql,new {type}); 
        }

        public string GetGradeName(int id)
        {
            var sql = "SELECT name FROM Grade WHERE Id=@id  and isdeleted=0 ";
            var res = Query<string>(sql, new { id });
            return res.FirstOrDefault(); 
        }
        public int  GetGradeId(string  gradeName) {
            var sql = "SELECT id FROM Grade WHERE name=@gradeName  and isdeleted=0 ";
            var res = Query<int>(sql, new { gradeName });
            return res.FirstOrDefault();
        }
        public IEnumerable<Category> GetCategories() {

            var sql = @"select * from Category  and IsDeleted=0 ";
            return Query<Category>(sql);

        }
        public string GetCategorieName(int id) {
            var sql = "SELECT CategoryName FROM Category WHERE Id=@id  and IsDeleted=0 ";
            var res = QueryFirst<string>(sql, new { id });
            return res;
        } 
        public Category GetCategorie(int id) {
            var sql = "SELECT * FROM Category WHERE Id=@id  and IsDeleted=0 ";
            var res = QueryFirst<Category>(sql, new { id });
            return res;
        }
    }
}
