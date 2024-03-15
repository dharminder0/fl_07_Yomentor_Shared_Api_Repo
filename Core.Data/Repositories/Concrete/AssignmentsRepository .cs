using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class AssignmentsRepository:DataRepository<Assignments>,IAssignmentsRepository {
        public async Task<int> InsertAssignment(Assignments assignment) {
            var sql = @"
        IF NOT EXISTS (SELECT 1 FROM Assignments WHERE Id = @Id)
        BEGIN
            INSERT INTO Assignments
            (
                Teacherid,
                Title,
                Description,
                GradeId,
                Subjectid,
                Isfavorite,
                createdate,
                updatedate,
                Isdeleted

            )
            VALUES
            (
                @Teacherid,
                @Title,
                @Description,
                @GradeId,
                @Subjectid,
                @Isfavorite,
                getutcdate(),
                getutcdate(),
                @Isdeleted
            );

            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SELECT Id FROM Assignments WHERE Id = @Id;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, assignment);
        }

        public async Task<int> UpdateAssignment(Assignments assignment) {
            var sql = @"
        IF EXISTS (SELECT 1 FROM Assignments WHERE Id = @Id)
        BEGIN
            UPDATE Assignments
            SET
                Teacherid = @Teacherid,
                Title = @Title,
                Description = @Description,
                GradeId = @GradeId,
                Subjectid = @Subjectid,
                Isfavorite = @Isfavorite,
                Isdeleted = @Isdeleted
            WHERE
                Id = @Id;

            SELECT Id FROM Assignments WHERE Id = @Id;
        END
        ELSE
        BEGIN
            SELECT -1;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, assignment);
        }

        public IEnumerable<Assignments> GetAssignments(int id)
        {
            var sql = $@"Select * from Assignments where Id=@id";
            return Query<Assignments>(sql,new {id});
        }

        public async Task<List<Assignments>> GetAllAssignments( int teacherid)
        {

            var sql = $@"Select * from Assignments where  teacherid=@teacherid ";
            return (List<Assignments>)await QueryAsync<Assignments>(sql,new {teacherid});
        }
        public async Task<IEnumerable<Assignments>> GetAssignmentsByBatch(ListRequest request) {

            var sql = $@"select  A.*  from Assignments A join  student_assignments SA on A.id=SA.assignmentid where SA.batchid=@batchId ";
            if (request.PageIndex > 0 && request.PageIndex > 0) {
                sql += $@" ORDER BY A.Id DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }

            return (List<Assignments>)await QueryAsync<Assignments>(sql,request);
        }

    }
}
