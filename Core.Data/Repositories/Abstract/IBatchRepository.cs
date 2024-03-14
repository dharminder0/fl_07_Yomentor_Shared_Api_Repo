using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IBatchRepository : IDataRepository<Batch> {

        Task<int> InsertBatchDetails(BatchDetailRequest batchDetailRequest);

        List<Batch> GetBatchDetailsbyId(int teacherId);
        Task<int> UpdateBatchDetails(BatchDetailRequest batchDetailRequest);
        IEnumerable<Batch> GetBatchDetailsbybatchId(int batchId);
        List<Batch> GetBatchDetails(int teacherId, int statusId);
    }
}
