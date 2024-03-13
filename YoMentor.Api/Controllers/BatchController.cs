﻿using Core.Business.Entities.RequestModels;
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
        public IActionResult GetBatchDetails(int teacherId) {
            var response = _batchService.BatchDetailsByTeacherId(teacherId);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="batchDetailRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest) {
            var response=  await _batchService.AddBatchDetails(batchDetailRequest);   
            return JsonExt(response);
        }
    }
}
