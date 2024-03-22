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
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Sevices.Concrete
{
    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IMediaFileRepository _mediaFileRepository; 
        public ReviewsService(IReviewsRepository reviewsRepository, IBatchRepository batchRepository, IMediaFileRepository mediaFileRepository)
        {
            _reviewsRepository = reviewsRepository;
            _batchRepository = batchRepository;
            _mediaFileRepository = mediaFileRepository;

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
                    try {
                        var studentImage = _mediaFileRepository.GetImage(item.AddedByUserId, MediaEntityType.Users);
                        review.StudentImage = studentImage.BlobLink
;                    } catch (Exception) {

                      
                    }
                    try {
                        var teacherImage = _mediaFileRepository.GetImage(item.AddedForUserId, MediaEntityType.Users);
                        review.TeacherImage = teacherImage.BlobLink;
                    } catch (Exception) {

                    }
                    review.Id = item.Id;
                    review.AddedForUserId = item.AddedForUserId;
                    review.AddedByUserId= item.AddedByUserId;
                    review.AddedForFirstName = item.AddedForFirstName;
                    review.AddedForLastName = item.AddedForLastName;
                    review.AddedByFirstName = item.AddedByFirstName;
                    review.AddedByLastName= item.AddedByLastName;
                    var batchDetails = _batchRepository.GetBatchNamebybatchId(item.BatchId);
                    review.BatchId = item.BatchId;
                    review.BatchTitle= batchDetails.ElementAt(0);
                    review.Rating = item.Rating;
                    review.Review= item.Review;
                    review.CreateDate = item.CreateDate;
                    if (item.UpdateDate == DateTime.MinValue)
                    {
                        review.UpdateDate = DateTime.UtcNow;
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
