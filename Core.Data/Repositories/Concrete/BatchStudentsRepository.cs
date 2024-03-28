using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;

namespace Core.Data.Repositories.Concrete {
    public class BatchStudentsRepository : DataRepository<BatchStudents>,IBatchStudentsRepository
    {
        public IEnumerable<BatchStudents> GetBatchStudentsbybatchId(int batchId)
        {
            var sql = $@"SELECT * FROM batch_students WHERE batchId=@batchId";
            return Query<BatchStudents>(sql, new { batchId });
        }
        public async Task<bool> UpdateEnrollmentStatus(int status, int Id, int batchId) {
            var sql = @" update batch_students set enrollmentstatus=@status  where studentid=@Id and BatchId= @batchId ";
            return await  ExecuteScalarAsync<bool>(sql, new { status, Id,batchId });   
        }

        public  async Task<int> InsertBatchStudent(BatchStudents batchStudent) {
        
            
                    var sql = $@"
                IF NOT EXISTS (SELECT 1 FROM Batch_Students WHERE StudentId = @StudentId AND BatchId = @BatchId)
                BEGIN
                    INSERT INTO Batch_Students
                    (
                        StudentId,
                        BatchId,
                        Enrollmentstatus,
                        CreateDate,
                        UpdateDate,
                        IsDeleted
                    )
                    VALUES
                    (
                        @StudentId,
                        @BatchId,
                        @Enrollmentstatus,
                        @CreateDate,
                        @UpdateDate,
                        @IsDeleted
                    );

                    SELECT SCOPE_IDENTITY();
                END
                ELSE
                BEGIN
                    SELECT Id FROM Batch_Students WHERE StudentId = @StudentId AND BatchId = @BatchId;
                END";

                    var parameters = new {
                        StudentId = batchStudent.StudentId,
                        BatchId = batchStudent.BatchId,
                        Enrollmentstatus = batchStudent.Enrollmentstatus,
                        CreateDate = batchStudent.CreateDate,
                        UpdateDate = batchStudent.UpdateDate,
                        IsDeleted = batchStudent.IsDeleted
                    };

                    return  await ExecuteScalarAsync<int>(sql, parameters);
                }

        public async Task<bool> DeleteBatchStudents(int batchId, DateTime date) {
            string datestring = date.ToString("yyyy/MM/dd");
            var sql = $@"DELETE FROM batch_students WHERE batchId = @batchId AND createdate = '{datestring}'";
            return await ExecuteScalarAsync<bool>(sql, new { batchId, datestring });
        }

        public async Task<BatchStudents> GetEnrollmentStatus(int batchId,int studentId) {
            var sql = @" select enrollmentstatus from batch_students where batchId=@batchId and studentId=@studentId ";
            return await QueryFirstAsync<BatchStudents>(sql, new { batchId, studentId });
        }
    }
}
