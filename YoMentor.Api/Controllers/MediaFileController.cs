using Core.Business.Entites.Utils;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Services.Abstract;
using Core.Business.Services.Concrete;
using Core.Common.Extensions;
using Hub.Common.Settings;
using Hub.Web.Api.Controllers;
using Hub.Web.Api.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using static Core.Business.Entities.DTOs.Enum;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaFileController : BaseApiController
    {
        private readonly IMediaFileService _mediaFileService;
        public MediaFileController(IMediaFileService mediaFileService)
        {
            _mediaFileService = mediaFileService;
        }

        [HttpPost]
        [Route("addMediaImage")]
        public IActionResult CreateMediaFile(MediaFileRequest requestMediaFile)
        {
            try
            {
                var response = _mediaFileService.CreateMediaFile(requestMediaFile);
                return JsonExt(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Used to get MediaFile List
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="mediaEntityType"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getByEntityId/{entityId}/")]
        [RequireAuthorization]
        public IActionResult GetMediaFile(int entityId, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.Image)
        {
            try
            {
                var response = _mediaFileService.GetMediaFile(entityId, mediaEntityType, mediaType);
                return JsonExt(response);
            }
            catch (Exception ex)
            {
                return JsonExt(ex.Message);
            }
        }

        [HttpPost]
        [Route("DeleteMediaFile")]
        [RequireAuthorization]
        public IActionResult DeleteMediaFile(object deleteMediaFileRequest)
        {
            List<DeleteMediaFileRequest> deleteMediaFileRequestList = new List<DeleteMediaFileRequest>();
            if (deleteMediaFileRequest != null && deleteMediaFileRequest.GetType().Name.EqualsCI("JArray"))
            {
                deleteMediaFileRequestList = JsonConvert.DeserializeObject<List<DeleteMediaFileRequest>>(deleteMediaFileRequest.ToString());
            }
            else if (deleteMediaFileRequest != null && deleteMediaFileRequest.GetType().Name.EqualsCI("JObject"))
            {
                var deleteMediaFileRequestobject = JsonConvert.DeserializeObject<DeleteMediaFileRequest>(deleteMediaFileRequest.ToString());
                if (deleteMediaFileRequestobject != null)
                {
                    deleteMediaFileRequestList.Add(deleteMediaFileRequestobject);
                }
            }
            try
            {
                var response = _mediaFileService.DeleteMediaFile(deleteMediaFileRequestList);
                return JsonExt(response);
            }
            catch (Exception ex)
            {
                return JsonExt(ex.Message);
            }
        }

    }
}
