﻿using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IBatchStudentsRepository
    {
        IEnumerable<BatchStudents> GetBatchStudentsbybatchId(int batchId);
    }
}