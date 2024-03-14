using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public IEnumerable<Assessments> GetAssessmentsList(int id)
        {
            var sql = @"select * from [dbo].[assessments] where id=@id";
            return Query<Assessments>(sql, new { id }).ToList();
        }

        public async Task<List<Assessments>> GetAssessmentsAllList(int teacherid)
        {
            var sql = @"select * from [dbo].[assessments] WHERE teacherid=@teacherid";
            return (List<Assessments>)await QueryAsync<Assessments>(sql,new { teacherid });
        }
        public async Task<IEnumerable<Assessments>> GetAssignmentsByBatch(int batchId) {

            var sql = $@" select  A.*  from Assessments A join  student_assessments SA on A.id=SA.assessmentid where SA.batchid=@batchId ";
            return await QueryAsync<Assessments>(sql, new { batchId });
          
        }
    }
}
