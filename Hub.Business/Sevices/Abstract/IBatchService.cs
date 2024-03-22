using Core.Business.Entities.DataModels;

using Core.Business.Entities.RequestModels;
using Core.Common.Data;

using Core.Business.Entities.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Business.Entities.Dto;

namespace Core.Business.Sevices.Abstract
{
    public interface IBatchService
    {


        Task<ActionMassegeResponse> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest);

        List<BatchStudentDetailsDto> GetStudentDetailsbyBatchId(int batchId);
        Task<List<BatchDto>> BatchDetails(BatchRequest request);
        Task<ActionMassegeResponse> UpdateBatchStatus(int batchStatus, int batchId);
        Task<ActionMassegeResponse> UpdateEnrollmentStatus(int status, int Id);




    }
}
