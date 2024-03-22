using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class BatchStudentsRepository : DataRepository<BatchStudents>,IBatchStudentsRepository
    {
        public IEnumerable<BatchStudents> GetBatchStudentsbybatchId(int batchId)
        {
            var sql = $@"SELECT * FROM batch_students WHERE batchId=@batchId";
            return Query<BatchStudents>(sql, new { batchId });
        }
        public async Task<bool> UpdateEnrollmentStatus(int status, int Id) {
            var sql = @" update batch_students set enrollmentstatus=@status  where id=@Id  ";
            return await  ExecuteScalarAsync<bool>(sql, new { status, Id });   
        }
    }
}
