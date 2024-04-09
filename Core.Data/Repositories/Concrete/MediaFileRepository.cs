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


        public IEnumerable<MediaFile> GetEntityMediaFile(int objectId, MediaEntityType entityTypeId,  MediaType mediaTypeid ) {
            var sql = $@"SELECT * FROM Media_File where EntityId = @objectId and  EntityTypeId = @entityTypeId";
        
            if (mediaTypeid != MediaType.None) {
                sql += $@" and MediaTypeId ={(int)mediaTypeid}";
            }
            sql += " order by 1 desc   ";
            return Query<MediaFile>(sql, new { objectId, entityTypeId });
        }
        public MediaFile GetImage(int objectId, MediaEntityType entityTypeId) {
            var sql = $@"SELECT * FROM Media_File where EntityId = @objectId and  EntityTypeId = @entityTypeId";
            return QueryFirst<MediaFile>(sql, new { objectId, entityTypeId });
        }

        public bool InsertInMediaFile(MediaFileRequest requestMediaFile) {

            var sql = @"

INSERT INTO Media_File	 
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
            var sql = @"IF not EXISTS(SELECT 1 from Media_File where EntityId = @EntityId and  EntityTypeId = @EntityTypeId  )

BEGIN
INSERT INTO Media_File	 
		   ( 
            EntityTypeId,
            EntityId,
            FileName,
            BlobLink,
            MediaTypeId,
            UpdatedOn
        
          )
     VALUES(
            @EntityTypeId,
            @EntityId,
            @FileName,
            @BlobLink,
            @MediaTypeId,
      
            Getdate())         
end
else
begin
update Media_File  set  BlobLink=@BlobLink , FileName= @FileName where  EntityId = @EntityId and  EntityTypeId = @EntityTypeId 
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
            var sql = $@" DELETE FROM Media_File where  bloblink = @bloblink ";

            return Execute(sql, new { entityId, entityTypeId, bloblink }) > 0;
        }
        public bool DeleteMediaFileV2(int entityId, int entityTypeId, string bloblink) {
            var sql = $@" DELETE FROM Media_File where  bloblink = @bloblink and entityId=@entityId and entityTypeId=@entityTypeId ";

            return Execute(sql, new { entityId, entityTypeId, bloblink }) > 0;
        }

        public bool UpdateMediaImage(MediaFileRequest obj) {

            var sql = @"
                        UPDATE Media_File  set DocUsageType = @1
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


        public bool MediaFileExists(int entityId, int entityTypeId,string fileName) {
            var sql = "SELECT COUNT(1) FROM Media_File WHERE EntityId = @EntityId AND EntityTypeId = @EntityTypeId and fileName=@fileName ";
            var result = ExecuteScalar<int>(sql, new { EntityId = entityId, EntityTypeId = entityTypeId,fileName=fileName });
            return result > 0;
        }


   
        public bool DeleteMediaFIle( int entityId,int entityTypeId) {
            var sql = @" delete from Media_File where EntityTypeId=@entityTypeId and EntityId=@entityId ";
            return ExecuteScalar<bool>(sql, new {entityId, entityTypeId});  
        }
    }
}


