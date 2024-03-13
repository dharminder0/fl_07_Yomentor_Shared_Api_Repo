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

        [HttpGet]
        [Route("GetAssignmentList")]

        public async Task<IActionResult> GetAssignmentList()
        {
            var response = await _assignmentsService.GetAllAssignments();
            return JsonExt(response);
        }
    }
}
