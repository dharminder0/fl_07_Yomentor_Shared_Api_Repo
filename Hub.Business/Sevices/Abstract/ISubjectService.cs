using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface ISubjectService {
        Task<List<SubjectResponse>> GetAllSubjects(int gradeId);

    }
}
