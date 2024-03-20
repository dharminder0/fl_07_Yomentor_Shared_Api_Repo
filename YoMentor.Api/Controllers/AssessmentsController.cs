using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Business.Sevices.Concrete;
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
     /// <param name="teacherid"></param>
     /// <returns></returns>
        [HttpPost]
        [Route("GetAssessmentsList/teacherid")]
        public async Task<IActionResult> GetAssessmentsAllList(StudentProgressRequestV2 request )
        {
            var response = await _service.GetAssessmentsAllList(request);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("AssignStudentAssessments")]
        [HttpPost]

        public async Task<IActionResult> AssignStudentAssessments(StudentAssessmentRequestV2 requestV2) {
            var response = await _service.AssignStudentAssessment(requestV2);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [Route("GetAssessmentsList/batchId")]
        [HttpPost]
        public async Task<IActionResult> GetAssessmentByBatch(ListRequest request) {
            var response = await _service.GetAssessmentByBatch(request);
            return JsonExt(response);
        }
    }
}
