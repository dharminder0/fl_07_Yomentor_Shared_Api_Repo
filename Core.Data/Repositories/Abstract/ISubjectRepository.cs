﻿using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface ISubjectRepository : IDataRepository<Subject> {
        Task<IEnumerable<Subject>> GetAllSubjects(int gradeId);
        string GetSubjectName(int subjectId);
        string GetIcon(int subjectId);
        Subject GetSubjectDetails(int subjectId);
    }
}
