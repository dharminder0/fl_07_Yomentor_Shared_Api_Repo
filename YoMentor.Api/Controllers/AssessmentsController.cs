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
        private readonly IAssessmentsService _service;
        public AssessmentsController(IAssessmentsService service)
        {
            _service=service;   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assessments"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> UpsertAssessments(AssessmentsRequest assessments) {
            var response=await _service.InsertOrUpdateAssessments(assessments);
            return JsonExt(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assessments"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAssessments/{id}")]
        public IActionResult GetAssessmentsList(int id)
        {
            var response =_service.GetAssessmentsList(id);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assessments"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAssessmentsList")]
        public async Task<IActionResult> GetAssessmentsAllList()
        {
            var response = await _service.GetAssessmentsAllList();
            return JsonExt(response);
        }
    }
}
