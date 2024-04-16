using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface ISkillTestService {
        Task<List<SkillTestResponse>> GetSkillTestList(SkillTestRequest skillTest);
        SkillTestResponse GetSkillTest(int id);
    }
}
