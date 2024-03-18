using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
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
        public ReviewsService(IReviewsRepository reviewsRepository)
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
        public async Task<List<ReviewResponse>> GetReviewResponse(ReviewRequest reviewRequest)
        {
            if (reviewRequest == null)
            {
                return null;
            }
            try
            {
                var res = await _reviewsRepository.GetReviewResponse(reviewRequest);
                if(res == null)
                {
                    return null;
                }
                ReviewResponse review = new ReviewResponse();
                List<ReviewResponse> reviews = new List<ReviewResponse>();
                foreach(var item  in res)
                {
                    review = new ReviewResponse();
                    review.Id = item.UserId;
                    review.UserId = item.UserId;
                    review.AddedByUserId= item.AddedByUserId;
                    review.AddedByUserId = item.AddedByUserId;
                    review.AddedByFirstName = item.AddedByFirstName;
                    review.AddedByLastName = item.AddedByLastName;
                    review.BatchId = item.BatchId;
                    review.BatchTitle= item.BatchTitle;
                    review.Rating = item.Rating;
                    review.Review= item.Review;
                    review.CreateDate = item.CreateDate;
                    if (item.UpdateDate == DateTime.MinValue)
                    {
                        item.UpdateDate = DateTime.UtcNow;
                    }
                    else
                    {
                        review.UpdateDate = item.UpdateDate;
                    }
                    reviews.Add(review);
                }
                return reviews;

        }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
