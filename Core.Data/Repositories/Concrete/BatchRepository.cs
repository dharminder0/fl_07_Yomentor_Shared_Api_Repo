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
              ,status
        
            
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
             ,1
            
          
            );

    SELECT SCOPE_IDENTITY() 
END
ELSE 
BEGIN
    SELECT Id FROM batch WHERE Name = @Name;
END ;";

            return  await ExecuteScalarAsync<int>(sql, batchDetailRequest);
        }
        

    }
}
