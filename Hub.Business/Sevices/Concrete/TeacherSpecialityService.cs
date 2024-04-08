using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;

namespace Core.Business.Sevices.Concrete {
    public class TeacherSpecialityService: ITeacherSpecialityService {
        private readonly ITeacherSpecialityRepository obj;
        public TeacherSpecialityService(ITeacherSpecialityRepository _obj)
        {
                obj = _obj;
        }
        public async  Task<ActionMassegeResponse> TeacherSpeciality(TeacherSpecialityRequest request) {
            if (request != null && request.GradeSubjectList !=null && request.GradeSubjectList.Any()) {
                await obj.DeleteTeacherSpeciality(request.TeacherId);

                TeacherSpeciality teacherSpecialityRequest = new TeacherSpeciality();
                teacherSpecialityRequest.TeacherId = request.TeacherId;
                foreach (var item in request.GradeSubjectList) {
                    teacherSpecialityRequest.SubjectId = item.SubjectId;    
                    teacherSpecialityRequest.GradeId = item.GradeId;
                    await obj.InsertTeacherSpeciality(teacherSpecialityRequest);
       

                }
                return new ActionMassegeResponse { Message = "Assigned_Successfully", Response = true };



            }
            return new ActionMassegeResponse { Message = "failed", Response = false };
        }
    }
}
