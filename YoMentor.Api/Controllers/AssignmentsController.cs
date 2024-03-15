using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : BaseApiController {
        private readonly IAssignmentsService _assignmentsService;
        public AssignmentsController(IAssignmentsService assignmentsService)
        {
            _assignmentsService = assignmentsService;  
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="assignmentsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> UpsertAssignments(AssignmentsRequest assignmentsRequest) {
            var response=await _assignmentsService.InsertOrUpdateAssignments(assignmentsRequest); 
            return JsonExt(response);   
        }

        [HttpGet]
        [Route("GetAssignment/{id}")]

        public IActionResult GetAssignment(int id)
        {
            var response= _assignmentsService.GetAssignment(id);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="teacherid"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAssignmentsList/teacherid")]

        public async Task<IActionResult> GetAssignmentList(StudentProgressRequest request)
        {
            var response = await _assignmentsService.GetAllAssignments(request);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
       
        [Route("AssignStudentAssignments")]
        [HttpPost]

        public async Task<IActionResult> AssignStudentAssignments(StudentAssignmentsRequest request) {
            var response = await _assignmentsService.AssignStudentAssignments(request);
            return JsonExt(response);
        }/// <summary>
        /// 
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAssignmentsList/batchId")]
        public async Task<IActionResult> GetAssignmentsByBatch(ListRequest request) {
            var response=await _assignmentsService.GetAssignmentsByBatch(request);  
            return JsonExt(response);   
        }
    }
}
