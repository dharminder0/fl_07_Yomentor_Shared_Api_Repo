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

        public  async Task<Assignments> GetAssignments(int id)
        {
            var sql = $@"Select * from Assignments where Id=@id and IsDeleted=0  or  IsDeleted is null";
            return QueryFirst<Assignments>(sql, new { id });
        }

        public async Task<List<Assignments>> GetAllAssignments(StudentProgressRequestV2 request) {

            var sql = $@"Select * from Assignments where  teacherid=@TeacherId and IsDeleted=0  or  IsDeleted is null";
            if (request.GradeId > 0)
            {
                sql += $@" and GradeId=@GradeId";
            }
            if (request.SubjectId > 0)
            {
                sql += $@" and SubjectId=@SubjectId";
            }
            if (request.PageIndex > 0 && request.PageIndex > 0) {
                sql += $@" ORDER BY Id DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }
           var res= (List<Assignments>)await QueryAsync<Assignments>(sql, request);
            return res;
        }
            public async Task<IEnumerable<Assignments>> GetAssignmentsByBatch(ListRequest request) {

            var sql = $@"SELECT A.*,DistinctAssignments.AssignedDate
FROM (
    SELECT DISTINCT SA.AssignmentId, cast(SA.AssignedDate as date) as AssignedDate
    FROM Assignments A
    JOIN student_assignments SA ON A.id = SA.assignmentid
    WHERE SA.batchid =@batchId and IsDeleted=0  or  IsDeleted is null ";
            if (request.StudentId>0)
            { sql += $@" and SA.StudentId = @StudentId"; }
                  sql+=$@" ) AS DistinctAssignments
                JOIN Assignments A ON A.id = DistinctAssignments.AssignmentId";
           
                if (request.PageIndex > 0 && request.PageIndex > 0) {
                    sql += $@" ORDER BY A.Id DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

                }

                var res= (List<Assignments>)await QueryAsync<Assignments>(sql, request);
            return res;
            }
        public bool DeleteAssessment(int Id) {
            var sql = @"update Assignments set isdeleted=1 where id=@id ";
            return ExecuteScalar<bool>(sql, new { Id });
        }
    }
}
