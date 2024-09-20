using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IBannersRepository : IDataRepository<Banners> {
        
        IEnumerable<Banners> GetBanners(int userType, int pageType);
    }
}
