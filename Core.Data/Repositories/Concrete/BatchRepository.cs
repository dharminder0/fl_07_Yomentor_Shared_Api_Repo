using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class BatchRepository : DataRepository<Batch>, IBatchRepository
    {
        public List<Batch> GetBatchDetailsbyId(int teacherId)
        {
            var sql =$"Select * from dbo.Batch where teacherid=@teacherId and isdeleted=0";
            var res=  Query<Batch>(sql, new {teacherId});
            return (List<Batch>)res;    
        }
        public IEnumerable<Batch> GetBatchDetailsbybatchId(int batchId)
        {
            var sql = $"Select * from dbo.Batch where Id=@batchId and isdeleted=0";
            return Query<Batch>(sql, new { batchId });
        }
        public async Task<int> InsertBatchDetails(BatchDetailRequest batchDetailRequest) {
           
            var sql = $@"IF NOT EXISTS (SELECT 1 FROM batch WHERE Name = @Name)
BEGIN
    INSERT INTO batch
           (
            
              Name
             ,Description        
             ,TeacherId
             ,GradeId
             ,SubjectId
             ,TuitionTime
             ,Fee
             ,FeeType
             ,StudentCount
             ,Days 
             ,isdeleted
       
            )
     VALUES
           (
        
            @Name
            ,@Description
            ,@TeacherId
            ,@GradeId
            ,@SubjectId
            ,@ClassTime
            ,@Fee
            ,@FeeType
            ,@NumberOfStudents
            ,@Days
            ,0
        
    
            );

    SELECT SCOPE_IDENTITY() 
END
ELSE 
BEGIN
    SELECT Id FROM batch WHERE Name = @Name;
END ;";

            return  await ExecuteScalarAsync<int>(sql, batchDetailRequest);
        }
        public async Task<int> UpdateBatchDetails(BatchDetailRequest batchDetailRequest) {

            var sql = @"
        IF EXISTS (SELECT 1 FROM batch WHERE Id = @Id)
        BEGIN
            UPDATE batch
            SET
                Description = @Description,
                TeacherId = @TeacherId,
                GradeId = @GradeId,
                SubjectId = @SubjectId,
                TuitionTime = @ClassTime,
                Fee = @Fee,
                FeeType = @FeeType,
                StudentCount = @NumberOfStudents,
                Days = @Days
            WHERE
                Id = @Id;

            SELECT Id FROM batch WHERE Id = @Id;
        END
        ELSE
        BEGIN
        
            SELECT -1;
        END;";

            return await ExecuteScalarAsync<int>(sql, batchDetailRequest);
        }



    }
}
