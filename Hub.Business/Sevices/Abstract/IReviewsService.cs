﻿using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract
{
    public interface IReviewsService
    {
        Task<ActionMessageResponse> InsertOrUpdateReviews(Reviews reviews);
        Task<List<ReviewResponse>> GetReviewResponse(ReviewRequest reviewRequest);
    }
}
