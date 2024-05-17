using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class StudentAssignmentsRepository: DataRepository<Student_Assignments>,IStudentAssignmentsRepository {
        public async Task<int> InsertStudentAssignment(Student_Assignments studentAssignment) {
            var sql = @" 
            INSERT INTO Student_Assignments
            (
                StudentId,
                BatchId,
                AssignmentId,
                status,
                AssignedDate
            )
            VALUES
            (
                @StudentId,
                @BatchId,
                @AssignmentId,
                @Status,
                GetUtcDate()
            );

            SELECT SCOPE_IDENTITY();";

          var res= await ExecuteScalarAsync<int>(sql, studentAssignment);
            return res;
        }

        public async Task<int> UpdateStudentAssignment(Student_Assignments studentAssignment) {
            var sql = @"
        IF EXISTS (SELECT 1 FROM Student_Assignments WHERE Id = @Id)
        BEGIN
            UPDATE Student_Assignments
            SET
                StudentId = @StudentId,
                BatchId = @BatchId,
                AssignmentId = @AssignmentId,
                status = @Status
            WHERE
                Id = @Id;

            SELECT Id FROM Student_Assignments WHERE Id = @Id;
        END
        ELSE
        BEGIN
            SELECT -1;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, studentAssignment);
        }
        public async Task<bool> DeleteStudentAssignment(int batchId) {
            var sql = @" delete from student_assignments where batchId=@batchId ";
            return await  ExecuteScalarAsync<bool>(sql,new { batchId });       

        }
        public bool DeleteStudentAssignments(int batchId, int assignmentId) {
            var sql = @" delete from student_assignments where batchId=@batchId and assignmentid=@assignmentId ";
            return  ExecuteScalar<bool>(sql, new { batchId,assignmentId });

        }

    }
}
