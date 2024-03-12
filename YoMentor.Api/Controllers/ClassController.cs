using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Youmentor.Web.Api.Controllers {
   
    [Route("api/class")]
    [ApiController]
   
    public class ClassController : BaseApiController {
        private readonly IclassService _classService;
        public ClassController(IclassService classService)
        {
            _classService = classService;
                
        }
        // GET: api/<ClassController>
        [HttpGet]
         public IActionResult GetClass()
        {
            var response=   _classService.GetUsers();   
            return Ok(response);    
        }

        [HttpGet]
        [Route("Teacher/{Id}")]
        public IActionResult GetTeacherById (int Id)
        {
            try
            {
                var response = _classService.GetTeacherById(Id);
                return JsonExt(response);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }
    }
}
