using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IBatchRepository
    {

        Task<int> InsertBatchDetails(BatchDetailRequest batchDetailRequest);

        List<Batch> GetBatchDetailsbyId(int teacherId);
        Task<int> UpdateBatchDetails(BatchDetailRequest batchDetailRequest);
    }
}
