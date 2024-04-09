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
        private readonly BlobStorageService _blobStorageService = new BlobStorageService();
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

        public IActionResult DeleteMediaFile(object deleteMediaFileRequest) {
            List<DeleteMediaFileRequest> deleteMediaFileRequestList = new List<DeleteMediaFileRequest>();

            if (deleteMediaFileRequest != null) {
                if (deleteMediaFileRequest is JArray) {
                    deleteMediaFileRequestList = JsonConvert.DeserializeObject<List<DeleteMediaFileRequest>>(deleteMediaFileRequest.ToString());
                }
                else if (deleteMediaFileRequest is JObject) {
                    var deleteMediaFileRequestobject = JsonConvert.DeserializeObject<DeleteMediaFileRequest>(deleteMediaFileRequest.ToString());
                    if (deleteMediaFileRequestobject != null) {
                        deleteMediaFileRequestList.Add(deleteMediaFileRequestobject);
                    }
                }
                else if (deleteMediaFileRequest is string) {
                    // Handle the case where deleteMediaFileRequest is a string
                    var stringRequest = new DeleteMediaFileRequest { Bloblink = deleteMediaFileRequest.ToString() }; // Assuming DeleteMediaFileRequest has a property for string data
                    deleteMediaFileRequestList.Add(stringRequest);
                }
                else {
                    // Handle any other unexpected types of deleteMediaFileRequest
                    return JsonExt("Invalid request format: unsupported object type.");
                }
            }

            try {
                var response = _mediaFileService.DeleteMediaFile(deleteMediaFileRequestList);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex.Message);
            }
        }

        /// <summary>
        [HttpPost]
        [Route("Blob/UploadFile")]
    
        public async Task<IActionResult> UploadFile(MediaEntityType mediaEntityType = MediaEntityType.None) {
            try {
                FileUploadResponse response = null;
                var formCollection = await Request.ReadFormAsync();
                var fileList = formCollection.Files;

                if (fileList != null) {
                    foreach (var item in fileList) {
                        using (var memoryStream = new MemoryStream()) {
                            await item.CopyToAsync(memoryStream);
                            var fileBytes = memoryStream.ToArray();

                            if (fileBytes != null) {
                                var fileName = item.FileName.Trim('\"');
                                fileName = fileName.Replace(" ", "").Replace("-", "");
                                string _blobContainerName = GlobalSettings.BlobContainerName;
                               
                              


                                response = _blobStorageService.UploadFileToBlob(fileName, fileBytes, _blobContainerName);
                            }
                        }
                    }
                }

                if (response != null) {
                    var serializer = new JsonSerializer { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                    var json = JObject.FromObject(response, serializer);
                    return JsonExt(new { data = json, message = "Upload success" });
                }
                else {
                    return JsonExt(new { data = (FileUploadResponse)null, message = "Upload failed" });
                }
            } catch (Exception ex) {
                return JsonExt(new { data = (string)null, message = ex.ExtractInnerException() });
          
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobLink"></param>
        /// <param name="entityId"></param>
        /// <param name="entityTypeId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteMediaFileV2")]

        public IActionResult DeleteMediaFile(string blobLink, int entityId,int entityTypeId) {
        var response=_mediaFileService.DeleteMediaFileV2(blobLink, entityId, entityTypeId); 
            return JsonExt(response);   
            
        }
    }
}
