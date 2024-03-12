using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : BaseApiController {
        private readonly ISubjectService _subService;

        public SubjectController(ISubjectService subService) {
            _subService = subService;

        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAllSubjects(int gradeId) {
            var response = await _subService.GetAllSubjects(gradeId);
            return JsonExt(response);


        }
    }
}
