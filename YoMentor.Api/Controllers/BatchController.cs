﻿using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Hub.Web.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
  
namespace YoMentor.Api.Controllers
{
    [Route("api/Batch")]
    [ApiController]
    public class BatchController : BaseApiController
    {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService)
        {
         _batchService = batchService;  
        }

        [HttpPost]
        [Route("BatchListbyTeacherId")]
       
        public async Task<IActionResult> GetBatchDetails(BatchRequest request ) {
            try
            {
                var response =await  _batchService.BatchDetails(request);
                return JsonExt(response);
            }
            catch (Exception ex) { 
            return JsonExt(ex);
            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="batchDetailRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest) {
            try
            {
                var response = await _batchService.AddBatchDetails(batchDetailRequest);
                return JsonExt(response);
            }
            catch (Exception ex)
            {
                return JsonExt(ex);
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="batchDetailRequest"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StudentList/{batchId}")]
        public IActionResult GetStudentDetailsbyBatchId(int batchId)
        {
            var response = _batchService.GetStudentDetailsbyBatchId(batchId);
            return JsonExt(response);
        }
     
    }
}
