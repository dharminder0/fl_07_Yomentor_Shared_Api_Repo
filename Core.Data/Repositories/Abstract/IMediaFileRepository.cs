using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Contracts;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Abstract {
    public interface IMediaFileRepository : IDataRepository<MediaFile> {
        IEnumerable<MediaFile> GetEntityMediaFile(int objectId, MediaEntityType entityTypeId, MediaType mediaTypeid = MediaType.None);
        MediaFile GetImage(int objectId, MediaEntityType entityTypeId);
        bool InsertInMediaFile(MediaFileRequest requestMediaFile);
        bool UpsertMediaFile(MediaFileRequest requestMediaFile);
        bool DeleteMediaFile(int entityId, int entityTypeId, string bloblink);
        public bool UpdateMediaImage(MediaFileRequest obj);
        bool DeleteMediaFileV2(int entityId, int entityTypeId, string bloblink);
        bool DeleteMediaFIle(int entityId, int entityTypeId);



    }
}

