﻿using Core.Business.Entities.DataModels;

using Core.Business.Entities.RequestModels;
using Core.Common.Data;

using Core.Business.Entities.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract
{
    public interface IBatchService
    {


        Task<ActionMassegeResponse> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest);

        List<BatchDto> BatchDetailsByTeacherId(int TeacherId);

    }
}
