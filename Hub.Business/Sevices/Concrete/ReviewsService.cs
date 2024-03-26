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
        private readonly IUserRepository _userRepository;   
        public ReviewsService(IReviewsRepository reviewsRepository, IBatchRepository batchRepository, IMediaFileRepository mediaFileRepository, IUserRepository userRepository)
        {
            _reviewsRepository = reviewsRepository;
            _batchRepository = batchRepository;
            _mediaFileRepository = mediaFileRepository;
            _userRepository = userRepository;

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
                        if (reviewRequest.AddedBy > 0) {
                            var studentImage = _mediaFileRepository.GetImage(item.AddedBy, MediaEntityType.Users);
                            if (studentImage != null) {
                                review.StudentImage = studentImage.BlobLink;
                            }
                                }
;                    } catch (Exception) {

                      
                    }
                    try {
                        if (reviewRequest.AddedFor > 0) {
                            var teacherImage = _mediaFileRepository.GetImage(item.AddedFor, MediaEntityType.Users);
                            if (teacherImage != null) {
                                review.TeacherImage = teacherImage.BlobLink;
                            }
                        }
                    } catch (Exception) {

                    }
                    try {
                        if (reviewRequest.AddedFor > 0) {
                            var teacherInfo = await _userRepository.GetUser(reviewRequest.AddedFor);
                            review.AddedForFirstName = teacherInfo.Firstname;
                            review.AddedForLastName = teacherInfo.Lastname;
                            review.AddedForUserId = teacherInfo.Id;
                        }

                    } catch (Exception) {

                        throw;
                    }
                    try {
                        if (reviewRequest.AddedBy > 0) {
                            var studentInfo = await _userRepository.GetUser(reviewRequest.AddedBy);
                            review.AddedByFirstName = studentInfo.Firstname;
                            review.AddedByLastName = studentInfo.Lastname;
                            review.AddedByUserId = studentInfo.Id;
                        }
                    } catch (Exception) {

                        throw;
                    }
                  
                 
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
