using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IFavouriteBatchRepository:IDataRepository<FavouriteBatch> {
        Task<int> InsertOrUpdateFavouriteBatch(FavouriteBatch batch);
        Task<FavouriteBatch> GetFavouriteStatus(int userId, int entityTypeId);
        Task<bool> UpdateStatus(int userId, int entityId);

    }
}
