﻿using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Concrete
{
    public class BatchRepository : DataRepository<Batch>, IBatchRepository
    {
        public async Task<IEnumerable<Batch>> GetBatchDetailsbyId(BatchRequest request)
        {
            var sql = @" select B.* from Batch B  ";
            if (request.UserType == 3) {
                sql += $@" join  batch_students BS on B.id=BS.batchid  where BS.studentId= @userId ";
            }
            if (request.UserType == 1) {
                sql += $@" where B.teacherId= @userId ";
            }

            if (request.StatusId!= null) {
                sql += $@" and B.status in @StatusId ";
            }
            if(request.PageIndex  > 0 && request.PageIndex > 0) {
                sql += $@" ORDER BY B.status DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }
            return  await QueryAsync<Batch>(sql, request);
        }

        public IEnumerable<int> CounterStudent(int batchId)
        {
            var sql = $"select Count(studentid) from batch_students where batchId=@batchId";
            var res = Query<int>(sql, new {batchId});   
            return (IEnumerable<int>)res;
        }
        public List<Batch> GetBatchDetails(int teacherId, int statusId)
        {
            var sql = $"Select * from dbo.Batch where teacherid=@teacherId and status=@statusId and isdeleted=0";
            var res = Query<Batch>(sql, new { teacherId ,statusId});
            return (List<Batch>)res;
        }
        public IEnumerable<Batch> GetBatchDetailsbybatchId(int batchId)
        {
            var sql = $"Select * from dbo.Batch where Id=@batchId and isdeleted=0";
            return Query<Batch>(sql, new { batchId });
        }
        public IEnumerable<string> GetBatchNamebybatchId(int batchId)
        {
            var sql = $"Select name  from dbo.Batch where Id=@batchId";
            var res= Query<string>(sql,new { batchId });
            return res;
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
               ,CreateDate
              ,StartDate
             ,FeeType
             ,StudentCount
             ,Days 
             ,isdeleted
             ,[status]
       
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
            ,@CreateDate
            ,@Date
            ,@FeeType
            ,@NumberOfStudents
            ,@Days
            ,'0'
            ,'1');

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


        public async Task<IEnumerable<Batch>> GetBatchDetailsbyStudentId(int studentId) {
            var sql = $"select  B. *, BS.studentid from Batch B join batch_students BS on B.id=BS.batchid  where BS.studentid=@studentId  and B.isdeleted=0";
            return  await QueryAsync<Batch>(sql , new {studentId});    
            
        }
        public  async Task<List<Batch>> GetBatchDetailsV2(int teacherId, int statusId) {
            var sql = $"Select * from dbo.Batch where teacherid=@teacherId and status=@statusId and isdeleted=0";
            var res =await  QueryAsync<Batch>(sql, new { teacherId, statusId });
            return (List<Batch>)res;
        }
        public async Task<bool> UpdateBatchStatus(int batchStatus, int batchId) {
            var sql = @" update Batch set status=@batchStatus where id=@batchId  ";
            return await  ExecuteScalarAsync<bool>(sql, new { batchStatus,batchId});  
        }

        
    }
}
