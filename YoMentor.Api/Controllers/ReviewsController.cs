using Autofac.Core;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : BaseApiController
    {
        private readonly IReviewsService _reviewsService;
        public ReviewsController(IReviewsService reviewsService)
        {
            _reviewsService = reviewsService;       
        }

        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> UpsertReviews(Reviews reviews)
        {
            var response = await _reviewsService.InsertOrUpdateReviews(reviews);
            return JsonExt(response);
        }

        [HttpPost]
        [Route("GetReviews")]
        public async Task<IActionResult>GetReviews(ReviewRequest reviewRequest)
        {
            var response = await _reviewsService.GetReviewResponse(reviewRequest);
            return JsonExt(response);
        }
    }
}
