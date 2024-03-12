using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IBatchRepository
    {
       List<Batch> GetBatchDetailsbyId(int TeacherId);
    }
}
