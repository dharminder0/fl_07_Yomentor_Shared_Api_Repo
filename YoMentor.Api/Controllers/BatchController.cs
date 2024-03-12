using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BatchController : BaseApiController
    {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService)
        {
         _batchService = batchService;  
        }

        [HttpGet]
        [Route("OpenBatchbyTeacherId")]
        public IActionResult GetBatchDetails(int teacherId)
        {
            var response = _batchService.BatchDetailsByTeacherId(teacherId);
            return JsonExt(response);
        }

    }
}
