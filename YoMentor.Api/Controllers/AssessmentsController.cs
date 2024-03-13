using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
   
namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AssessmentsController : BaseApiController {
        public AssessmentsController(IAssessmentsService service)
        private readonly IAssessmentsService _service;
        {
            _service=service;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assessments"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Upsert")]
        public async Task<IActionResult> UpsertAssessments(AssessmentsRequest assessments) {
            var response=await _service.InsertOrUpdateAssessments(assessments);
            return JsonExt(response);
        }
    }
}
