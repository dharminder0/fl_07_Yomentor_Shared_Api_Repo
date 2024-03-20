using Core.Business.Entities.DataModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public class StudentAssessmentRepository :DataRepository<StudentAssessment>,IStudentAssessmentRepository{
        public async Task<int> InsertStudentAssessment(StudentAssessment studentAssessment) {
            var sql = @" INSERT INTO Student_Assessments
        (
            StudentId,
            BatchId,
            AssessmentId,
            Status,
            Marks,
            AssignedDate
        )
        VALUES
        (
            @StudentId,
            @BatchId,
            @AssessmentId,
            @Status,
            @Marks,
            GetUtcdate()
        );

        SELECT SCOPE_IDENTITY(); ";
        var res = await ExecuteScalarAsync<int>(sql, studentAssessment);
            return res;
        }

        public async Task<int> UpdateStudentAssessment(StudentAssessment studentAssessment) {
            var sql = @"
    IF EXISTS (SELECT 1 FROM Student_Assessments WHERE Id = @Id)
    BEGIN
        UPDATE Student_Assessments
        SET
            StudentId = @StudentId,
            BatchId = @BatchId,
            AssessmentId = @AssessmentId,
            Status = @Status,
            Marks = @Marks
        WHERE
            Id = @Id;

        SELECT Id FROM Student_Assessments WHERE Id = @Id;
    END
    ELSE
    BEGIN
        SELECT -1;
    END;
    ";

            return await ExecuteScalarAsync<int>(sql, studentAssessment);
        }

    }
}
