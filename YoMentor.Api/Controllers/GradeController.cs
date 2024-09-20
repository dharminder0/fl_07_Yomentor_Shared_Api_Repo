using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : BaseApiController {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;   
                
        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetAllGrades(int type) {
            var response = await  _gradeService.GetAllGrades(type);
            return JsonExt(response);


        }
        [HttpGet]
        [Route("Category/list")]
        public IActionResult GetCategory() {
            var response =  _gradeService.GetCategory();
            return JsonExt(response);


        }
    }
}
