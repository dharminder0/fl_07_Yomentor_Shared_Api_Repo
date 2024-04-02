using Core.Business.Entities.DataModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public class TeacherSpecialityRepository : DataRepository<TeacherSpeciality>, ITeacherSpecialityRepository {

        public async Task<int> InsertTeacherSpeciality(TeacherSpeciality teacherSpeciality) {
            var sql = @"
       
            INSERT INTO teacher_speciality
            (
                SubjectId,
                TeacherId,
                GradeId
            )
            VALUES
            (
                @SubjectId,
                @TeacherId,
                @GradeId
            );

            SELECT SCOPE_IDENTITY();
      
    ";

            return await ExecuteScalarAsync<int>(sql, teacherSpeciality);
        }
        public async Task<int> DeleteTeacherSpeciality(int teacherId) {
            var sql = @"  delete from teacher_speciality where teacherId=@teacherId ";
            return await  ExecuteScalarAsync<int>(sql, new { teacherId });
        }
        public async Task<IEnumerable<TeacherSpeciality>> GetTeacherSpeciality(int userid) {
            var sql = @" select * from Teacher_Speciality where teacherId=@userid ";
            return await QueryAsync<TeacherSpeciality>(sql, new { userid });
        }
    }
}
