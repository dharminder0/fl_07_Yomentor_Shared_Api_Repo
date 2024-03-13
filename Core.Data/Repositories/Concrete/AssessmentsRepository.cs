using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class AssessmentsRepository :DataRepository<Assessments>,IAssessmentsRepository {
        public async Task<int> InsertAssessments(Assessments assignment) {
            var sql = @"
        IF NOT EXISTS (SELECT 1 FROM Assessments WHERE Id = @Id)
        BEGIN
            INSERT INTO Assessments
            (
                TeacherId,
                Title,
                Description,
                GradeId,
                subjectid,
                Maxmark,
                isfavorite,
                isdeleted,
                createdate,
                updatedate
            )
            VALUES
            (
                @TeacherId,
                @Title,
                @Description,
                @GradeId,
                @SubjectId,
                @Maxmark,
                @isfavorite,
                @isdeleted,
               GetUtcDate(),
                 GetUtcDate()
            );

            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SELECT Id FROM Assessments WHERE Id = @Id;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, assignment);
        }

        public async Task<int> UpdateAssessments(Assessments assignment) {
            var sql = @"
        IF EXISTS (SELECT 1 FROM Assessments WHERE Id = @Id)
        BEGIN
            UPDATE Assessments
            SET
                TeacherId = @TeacherId,
                Title = @Title,
                Description = @Description,
                GradeId = @GradeId,
                SubjcetId = @SubjcetId,
                Maxmark = @Maxmark,
                isfavorite = @isfavorite,
                isdeleted = @isdeleted,
                UpdatedDate = @UpdatedDate
            WHERE
                Id = @Id;

            SELECT Id FROM Assessments WHERE Id = @Id;
        END
        ELSE
        BEGIN
            SELECT -1;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, assignment);
        }

    }
}
