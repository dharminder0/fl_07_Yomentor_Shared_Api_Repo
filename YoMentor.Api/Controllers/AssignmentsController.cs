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
        [Route("api/Upsert")]
        public async Task<IActionResult> UpsertAssignments(AssignmentsRequest assignmentsRequest) {
            var response=await _assignmentsService.InsertOrUpdateAssignments(assignmentsRequest); 
            return JsonExt(response);   
        }
    }
}
