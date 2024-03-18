using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IReviewsRepository
    {
        Task<int> UpdateReviews(Reviews reviews);
        Task<int> InsertReviews(Reviews reviews);
        Task<IEnumerable<ReviewResponse>> GetReviewResponse(ReviewRequest reviewRequest);
    }
}
