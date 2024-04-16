﻿using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface ISkillTestRepository  : IDataRepository<SkillTest>{
        Task<IEnumerable<SkillTest>> GetSkillTestList(SkillTestRequest skillTest);
        SkillTest GetSkillTest(int Id);
        int GetSkillTestSumScore(int Id);
        int GetSkillTestUser(int Id);
    }
}
