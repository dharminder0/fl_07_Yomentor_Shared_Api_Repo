using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class FavouriteBatchRepository:DataRepository<FavouriteBatch>,IFavouriteBatchRepository {
        public  async Task<int> InsertOrUpdateFavouriteBatch(FavouriteBatch batch) {
            var sql = $@"
        IF EXISTS (SELECT 1 FROM favourite_batch WHERE UserId = @UserId AND EntityTypeId = @EntityTypeId AND EntityType = @EntityType)
        BEGIN
            UPDATE favourite_batch
            SET IsFavourite = @IsFavourite,
                CreatedDate = @CreatedDate
            WHERE UserId = @UserId AND EntityTypeId = @EntityTypeId AND EntityType = @EntityType
        END
        ELSE
        BEGIN
            INSERT INTO favourite_batch (UserId, IsFavourite, CreatedDate, EntityTypeId, EntityType)
            VALUES (@UserId, @IsFavourite, @CreatedDate, @EntityTypeId, @EntityType)
        END;

        SELECT SCOPE_IDENTITY();";

            return  await ExecuteScalarAsync<int>(sql, new {
                UserId = batch.UserId,
                IsFavourite = batch.IsFavourite,
                CreatedDate = batch.CreatedDate,
                EntityTypeId = batch.EntityTypeId,
                EntityType = batch.EntityType
            });
        }
        public async Task<FavouriteBatch> GetFavouriteStatus(int userId, int entityTypeId) {
            var sql = @" select * from favourite_batch where userid=@userId and EntityTypeId=@EntityTypeId ";
            return await QueryFirstAsync<FavouriteBatch>(sql, new { userId, entityTypeId });
        }
        public async Task<bool> UpdateStatus(int userId, int entityId) {
            var sql = @" update   favourite_batch   set IsFavourite=0 where userId=@userId and EntityTypeId=@entityId ";
            return  await ExecuteScalarAsync<bool>(sql, new { userId, entityId });
        }
    }
}
