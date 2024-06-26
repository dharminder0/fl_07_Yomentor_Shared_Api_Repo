﻿using Azure.Core;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Hub.Web.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/Batch")]
    [ApiController]
    public class BatchController : BaseApiController {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService) {
            _batchService = batchService;
        }

        [HttpPost]
        [Route("BatchListbyUserid")]

        public async Task<IActionResult> GetBatchDetails(BatchRequest request) {
            try {
                var response = await _batchService.BatchDetails(request);
                return JsonExt(response);
            } catch (Exception ex) {
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
            try {
                var response = await _batchService.AddBatchDetails(batchDetailRequest);
                return JsonExt(response);
            } catch (Exception ex) {
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
        public IActionResult GetStudentDetailsbyBatchId(int batchId) {
            var response = _batchService.GetStudentDetailsbyBatchId(batchId);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchStatus"></param>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateBatchStatus")]
        public async Task<IActionResult> UpdateBatchStatus(int batchStatus, int batchId) {
            var response = await _batchService.UpdateBatchStatus(batchStatus, batchId);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="batchStatus"></param>
        /// <param name="batchId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateEnrollmentStatus")]
        public async Task<IActionResult> UpdateEnrollmentStatus(int status, int studentid, int batchId) {
            var response = await _batchService.UpdateEnrollmentStatus(status, studentid,batchId);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("AssignedStudent")]
        public async Task<IActionResult> AssignedStudent(BatchStudentsRequest request) {
            var response = await _batchService.AssignBatchStudents(request);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="favouriteBatchRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AssignedFavouriteBatch")]
        public async Task<IActionResult> AssignedFavouriteBatch(FavouriteBatchRequest favouriteBatchRequest) {
            var response = await _batchService.InsertOrUpdateFavouriteBatch(favouriteBatchRequest);
            return JsonExt(response);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("UpdateFavouriteStatus")]
        public async Task<IActionResult> UpdateFavouriteStatus(int userId, int entityId) {
            var response= await _batchService.UpdateFavouriteStatus(userId, entityId);  
            return JsonExt(response);   
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BatchListbyEntity")]

        public async Task<IActionResult> GetBatchDetailsV2(BatchRequestV2 request) {
            try {
                var response = await _batchService.BatchDetails(request);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex);
            }
        }
        [HttpPost]
        [Route("Delete")]
        public IActionResult DeleteBatch(int id) {
            var response =  _batchService.DeleteBatch(id); 
            return JsonExt(response);
        }
    }
}
