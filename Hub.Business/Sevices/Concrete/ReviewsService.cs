using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete
{
   public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;
        public ReviewsService(IReviewsRepository reviewsRepository )
        {
            _reviewsRepository = reviewsRepository;
        }
        public async Task<ActionMessageResponse> InsertOrUpdateReviews(Reviews reviews)
        {
            if (reviews == null)
            {
                return new ActionMessageResponse { Success = false };
            }
            if (reviews.Id == 0)
            {
                int insertedId = await _reviewsRepository.InsertReviews(reviews);
                return new ActionMessageResponse { Content = insertedId, Message = "Reviews_Added", Success = true };
            }

            int id = await _reviewsRepository.UpdateReviews(reviews); 
            return new ActionMessageResponse { Content = id, Message = "Reviews_Updated", Success = true };
        }

    }
}
