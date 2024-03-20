using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Services.Abstract;
using Core.Data.Repositories.Abstract;
using Hub.Common.Settings;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using static Core.Business.Entities.DTOs.Enum;
using Task = System.Threading.Tasks.Task;

namespace Core.Business.Services.Concrete {
    public class MediaFileService : IMediaFileService
    {
       
        private readonly IUserRepository _userRepository;
        private readonly BlobStorageService _blobStorageService = new BlobStorageService();
      
        private readonly static string _blobContainerName = GlobalSettings.BlobContainerName;
        private readonly static string _blobStorageAccount = GlobalSettings.BlobStorageAccount;
        private readonly IMediaFileRepository _repository;
        public MediaFileService(IUserRepository userRepository, IMediaFileRepository repository)
        {
        
            _userRepository = userRepository;
            _repository = repository;

        }

        public bool CreateMediaFile(MediaFileRequest requestMediaFile)
        {
            try
            {
                if (requestMediaFile == null || (int)requestMediaFile.EntityTypeId == 0 || requestMediaFile.EntityId == 0 || string.IsNullOrWhiteSpace(requestMediaFile.FileName) || string.IsNullOrWhiteSpace(requestMediaFile.Bloblink))
                {
                    return false;
                }

           
           
                
                    requestMediaFile.FileName = GenerateSuitableFilename(requestMediaFile.FileName);
                    var response = GetMediaType(requestMediaFile.FileName);
                    requestMediaFile.MediaTypeId = response;
                    return _repository.InsertInMediaFile(requestMediaFile);
                


                throw new Exception("required parameter");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public IEnumerable<MediaFile> GetMediaFile(int entityId, MediaEntityType mediaEntityType, MediaType type)
        {

            var list = _repository.GetEntityMediaFile(entityId, mediaEntityType, type);
            return list;

        }

        public ActionMessageResponse DeleteMediaFile(List<DeleteMediaFileRequest> deleteMediaFileRequest)
        {
            bool isResponse = false;
            try {
                foreach (var item in deleteMediaFileRequest) {
                    isResponse = _repository.DeleteMediaFile(item.EntityId, (int)item.EntityTypeId, item.Bloblink);
           
                    if (isResponse) {
                        var isFileDeleted = _blobStorageService.DeleteFileToBlobAsync(item.Bloblink);
                    }
                    else {
                        return new ActionMessageResponse { Content = "", Message = "DeletedFailed", Success = true };
                    }
                    
                }
                return new ActionMessageResponse { Content = "", Message = "DeletedSuccessfully", Success = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FileUploadResponse DirectBlobUpload(List<FileRequest> files, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.Image) {
            string? cvFileName = null;
            byte[]? cvFileData = null;
            string BlobContainer = GlobalSettings.BlobContainerName; ;
            string prescription = null;
            var mediadetais = new object();

            try {
                if (files != null) {


                    foreach (var item in files) {
                        if (item.FileData != null) {
                            FileRequest fileRequest = new FileRequest();
                            fileRequest.FileName = item.FileName;
                            fileRequest.FileData = item.FileData;
                            cvFileData = Convert.FromBase64String(item.FileData);
                            cvFileName = fileRequest.FileName;
                            var fileNames = item.FileName.Trim('\"');
                            fileNames = fileNames.Replace(" ", "").Replace("-", "");

                            
                            var response = _blobStorageService.UploadFileToBlob(fileNames, cvFileData, BlobContainer);
                            return response;
                        }
                    }

                    return null;
                }

            } catch (Exception ex) {
            }

            return null;

        }




        public ActionMessageResponse UploadDocument(List<FileRequest> files, int objectId, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.Image)
        {
            string? cvFileName = null;
            byte[]? cvFileData = null;
            string BlobContainer = GlobalSettings.BlobContainerName; ;
            string prescription = null;
            var mediadetais = new object();

            try
            {
                if (files != null)
                {


                    foreach (var item in files)
                    {
                        if (item.FileData != null)
                        {
                            FileRequest fileRequest = new FileRequest();
                            fileRequest.FileName = item.FileName;
                            fileRequest.FileData = item.FileData;
                            cvFileData = Convert.FromBase64String(item.FileData);
                            cvFileName = fileRequest.FileName;
                            var fileNames = item.FileName.Trim('\"');
                            fileNames = fileNames.Replace(" ", "").Replace("-", "");

                         

                            var response = _blobStorageService.UploadFileToBlob(fileNames, cvFileData, BlobContainer);
                            MediaFileRequest mediaFileRequest = new MediaFileRequest();
                            mediaFileRequest.FileName = item.FileName;
                            mediaFileRequest.EntityId = objectId;
                            mediaFileRequest.Bloblink = response.FileLink;
                            var res = GetMediaType(response.FileLink);
                            if (mediaType == MediaType.None)
                            {

                                mediaFileRequest.MediaTypeId = GetMediaType(item.FileName);
                            }

                            mediaFileRequest.MediaTypeId = res;
                            mediaFileRequest.EntityTypeId = mediaEntityType;                          
                            mediadetais = CreateMediaFile(mediaFileRequest);
                        }
                    }

                    return new ActionMessageResponse { Success = true, Message = "upload successfully", Content = mediadetais };
                }

            }
            catch (Exception ex)
            {
            }

            return new ActionMessageResponse { Success = false, Message = "upload failed" };

        }


        public ActionMessageResponse UploadDocumentNew(List<FileRequest> files, int brandId, int objectId, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.None)
        {
            Task.Run(() =>
            {
                AsyncUploadDocumentNew(files, brandId, objectId, mediaEntityType, mediaType);
            });
            return new ActionMessageResponse { Success = true, Message = "upload successfully", Content = true };
        }



        private ActionMessageResponse AsyncUploadDocumentNew(List<FileRequest> files, int brandId, int objectId, MediaEntityType mediaEntityType,  MediaType mediaType = MediaType.None)
        {
            string? cvFileName = null;
            byte[]? cvFileData = null;
            string BlobContainer = GlobalSettings.BlobContainerName; ;
            string Prescription = null;
            try
            {
                if (files != null)
                {


                    foreach (var item in files)
                    {
                        if (item.FileData != null)
                        {
                            FileRequest fileRequest = new FileRequest();
                            fileRequest.FileName = item.FileName;
                            fileRequest.FileData = item.FileData;
                            cvFileData = Convert.FromBase64String(item.FileData);
                            cvFileName = fileRequest.FileName;
                            var fileNames = item.FileName.Trim('\"');
                            fileNames = fileNames.Replace(" ", "").Replace("-", "");

                            var response = _blobStorageService.UploadFileToBlob(fileNames, cvFileData, BlobContainer);
                            MediaFileRequest mediaFileRequest = new MediaFileRequest();
                            mediaFileRequest.FileName = item.FileName;
                            mediaFileRequest.EntityId = objectId;
                            mediaFileRequest.Bloblink = response.FileLink;
                            var res = GetMediaType(response.FileLink);
                            if (mediaType == MediaType.None)
                            {

                                mediaFileRequest.MediaTypeId = GetMediaType(item.FileName);
                            }

                            mediaFileRequest.MediaTypeId = res;




                            mediaFileRequest.EntityTypeId = mediaEntityType;

                            var mediaDetails = CreateMediaFile(mediaFileRequest);
                        }
                    }

                    return new ActionMessageResponse { Success = true, Message = "upload successfully" };
                }

            }
            catch (Exception ex)
            {
            }

            return new ActionMessageResponse { Success = false, Message = "upload failed" };

        }
    
        public MediaType GetMediaType(string fileName)
        {
            try
            {
                FileInfo fi = new FileInfo(fileName);
                string extension = fi.Extension.ToLower();
                switch (extension)
                {
                    case ".webm":
                    case ".mvm":
                    case ".mp3":
                    case ".mp4":
                    case ".mov":
                    case ".wmv":
                    case ".avi":
                    case ".flv":
                    case ".f4v":
                    case ".swf":
                    case ".avchd":

                    case ".mkv":


                        return MediaType.Video;
                        break;
                    case ".pdf":
                        return MediaType.Pdf;



                    default:
                        return MediaType.Image;

                }
            }
            catch (Exception)
            {

                return MediaType.None;
            }
        }
        public string UploadDocumentV2(List<FileRequest> files, int objectId, MediaEntityType mediaEntityType, int brandId, MediaType mediaType = MediaType.Image)
        {
            string? cvFileName = null;
            byte[]? cvFileData = null;
            string BlobContainer = GlobalSettings.BlobContainerName; ;
            string prescription = null;
            var mediadetais = new object();

            try
            {
                if (files != null)
                {
                    MediaFileRequest mediaFileRequest = new MediaFileRequest();

                    foreach (var item in files)
                    {
                        if (item.FileData != null)
                        {
                            FileRequest fileRequest = new FileRequest();
                            fileRequest.FileName = item.FileName;
                            fileRequest.FileData = item.FileData;
                            cvFileData = Convert.FromBase64String(item.FileData);
                            cvFileName = fileRequest.FileName;
                            var fileNames = item.FileName.Trim('\"');
                            fileNames = fileNames.Replace(" ", "").Replace("-", "");

                         
                            var response = _blobStorageService.UploadFileToBlob(fileNames, cvFileData, BlobContainer);

                            mediaFileRequest.FileName = item.FileName;
                            mediaFileRequest.EntityId = objectId;
                            mediaFileRequest.Bloblink = response.FileLink;
                            var res = GetMediaType(response.FileLink);
                            if (mediaType == MediaType.None)
                            {

                                mediaFileRequest.MediaTypeId = GetMediaType(item.FileName);
                            }

                            mediaFileRequest.MediaTypeId = res;
                            mediaFileRequest.EntityTypeId = mediaEntityType;
                            mediadetais = CreateMediaFile(mediaFileRequest);
                        }
                    }

                    return mediaFileRequest.Bloblink;
                }

            }
            catch (Exception ex)
            {
            }

            return null;

        }
        public bool DeleteFile(string fileName)
        {

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_blobStorageAccount);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_blobContainerName);
            var blob = cloudBlobContainer.GetBlobReference(fileName);
            blob.DeleteIfExistsAsync();
            return true;
        }
        private  string GenerateSuitableFilename(string originalFilename) {
            string fileExtension = System.IO.Path.GetExtension(originalFilename);  
            
       
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string suitableFilename = $"{timestamp}{fileExtension}";

            return suitableFilename;
        }

    }
}







