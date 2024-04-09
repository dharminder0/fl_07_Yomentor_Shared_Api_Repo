using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Services.Abstract {
    public interface IMediaFileService
    {
        bool CreateMediaFile(MediaFileRequest requestMediaFile);
        
        IEnumerable<MediaFile> GetMediaFile(int entityId, MediaEntityType mediaEntityType,MediaType type);
        ActionMessageResponse DeleteMediaFile(List<DeleteMediaFileRequest> deleteMediaFileRequest);
        FileUploadResponse DirectBlobUpload(List<FileRequest> files, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.Image);
        ActionMessageResponse UploadDocument(List<FileRequest> files, int objectId, MediaEntityType mediaEntityType, MediaType mediaType = MediaType.Image);
        ActionMessageResponse UploadDocumentNew(List<FileRequest> files, int brandId, int objectId, MediaEntityType mediaEntityType, MediaType mediaType=MediaType.Image );
 
        MediaType GetMediaType(string filename);
        string UploadDocumentV2(List<FileRequest> files, int objectId, MediaEntityType mediaEntityType, int brandId,MediaType mediaType = MediaType.Image);
        bool DeleteFile(string fileName);
        ActionMessageResponse DeleteMediaFileV2(string blobLink, int entityId, int EnitityTypeId);
    }
}
