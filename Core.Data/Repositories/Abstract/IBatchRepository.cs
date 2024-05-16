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

        Task<IEnumerable<Batch>> GetBatchDetailsbyId(BatchRequest request);
        Task<int> UpdateBatchDetails(BatchDetailRequest batchDetailRequest);
        IEnumerable<Batch> GetBatchDetailsbybatchId(int batchId);
        List<Batch> GetBatchDetails(int teacherId, int statusId);
        IEnumerable<int> CounterStudent(int batchId);
        Task<IEnumerable<Batch>> GetBatchDetailsbyStudentId(int studentId);
         Task<List<Batch>> GetBatchDetailsV2(int teacherId, int statusId);
        IEnumerable<string> GetBatchNamebybatchId(int batchId);
        Task<bool> UpdateBatchStatus(int batchStatus, int batchId);
        Task<IEnumerable<Batch>> GetBatchDetailsbyId(BatchRequestV2 request);
        bool DeleteBatch(int id);


    }
}
