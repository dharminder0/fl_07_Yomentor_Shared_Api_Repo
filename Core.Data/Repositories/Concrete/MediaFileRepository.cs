using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Concrete {
    public class MediaFileRepository : DataRepository<MediaFile>, IMediaFileRepository {

        public IEnumerable<MediaFile> GetEntityProfileMediaFile(int objectId, MediaEntityType entityTypeId, MediaType mediaTypeid = MediaType.None) {
            
            return GetEntityMediaFile(objectId, entityTypeId, mediaTypeid);


        }


        public IEnumerable<MediaFile> GetEntityMediaFile(int objectId, MediaEntityType entityTypeId,  MediaType mediaTypeid = MediaType.None) {
            var sql = $@"SELECT * FROM MediaFile where EntityId = @objectId and  EntityTypeId = @entityTypeId";
        
            if (mediaTypeid != MediaType.None) {
                sql += $@" and MediaTypeId ={(int)mediaTypeid}";
            }
            sql += " order by isdefault desc ";
            return Query<MediaFile>(sql, new { objectId, entityTypeId });
        }
        public MediaFile GetImage(int objectId, MediaEntityType entityTypeId) {
            var sql = $@"SELECT * FROM MediaFile where EntityId = @objectId and  EntityTypeId = @entityTypeId";
            return QueryFirst<MediaFile>(sql, new { objectId, entityTypeId });
        }

        public bool InsertInMediaFile(MediaFileRequest requestMediaFile) {

            var sql = @"

INSERT INTO MediaFile	 
		   (
            EntityTypeId,
            EntityId,
            FileName,
            BlobLink,
            MediaTypeId,
          UpdatedOn)
     VALUES(
            @EntityTypeId,
            @EntityId,
            @FileName,
            @BlobLink,
            @MediaTypeId,
            Getdate())    
";
            return Execute(sql, new {
                EntityTypeId = requestMediaFile.EntityTypeId,
                EntityId = requestMediaFile.EntityId,
                FileName = requestMediaFile.FileName,
                BlobLink = requestMediaFile.Bloblink,
                MediaTypeId = requestMediaFile.MediaTypeId,
                

            }) > 0;
        }

        public bool UpsertMediaFile(MediaFileRequest requestMediaFile) {
            var sql = @"IF not EXISTS(SELECT 1 from MediaFile where EntityId = @EntityId and  EntityTypeId = @EntityTypeId and MediaTypeId=@mediaTypeId)

BEGIN
INSERT INTO MediaFile	 
		   ( 
            EntityTypeId,
            EntityId,
            FileName,
            BlobLink,
            MediaTypeId,
            BrandId,
            DocUsageType)
     VALUES(
            @EntityTypeId,
            @EntityId,
            @FileName,
            @BlobLink,
            @MediaTypeId,
            @BrandId,
            @DocUsageType)    
end
else
begin
update MediaFile  set  BlobLink=@BlobLink , FileName= @FileName where  EntityId = @EntityId and  EntityTypeId = @EntityTypeId and MediaTypeId=@mediaTypeId
end 
";
            return Execute(sql, new {
                EntityTypeId = requestMediaFile.EntityTypeId,
                EntityId = requestMediaFile.EntityId,
                FileName = requestMediaFile.FileName,
                BlobLink = requestMediaFile.Bloblink,
                MediaTypeId = requestMediaFile.MediaTypeId,
               


            }) > 0;
        }



        public bool DeleteMediaFile(int entityId, int entityTypeId, string bloblink) {
            var sql = $@" DELETE FROM MediaFile where EntityId = @EntityId and  EntityTypeId = @EntityTypeId and bloblink = @bloblink ";

            return Execute(sql, new { entityId, entityTypeId, bloblink }) > 0;
        }

        public bool UpdateMediaImage(MediaFileRequest obj) {

            var sql = @"
                        UPDATE MediaFile  set DocUsageType = @DocUsageType
                        where  EntityTypeId =@EntityTypeId and 
                        EntityId = @EntityId and 
                        FileName = @FileName and 
                        BlobLink = @BlobLink and
                        MediaTypeId= @MediaTypeId and 
                        
                        brandid = @brandid                       
                        ";
            Execute(sql, obj);
            return true;
        }

    

    }
}


