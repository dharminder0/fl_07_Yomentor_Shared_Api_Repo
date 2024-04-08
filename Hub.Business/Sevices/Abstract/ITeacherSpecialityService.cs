using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface ITeacherSpecialityService {
        Task<ActionMassegeResponse> TeacherSpeciality(TeacherSpecialityRequest request);
    }
}
