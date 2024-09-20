using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using SqlKata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class BannersRepository : DataRepository<Banners>, IBannersRepository { 


        public IEnumerable<Banners> GetBanners(int userType,int pageType) {

            var sql = @"select * from Banners where userType=@userType and pageType=@pageType ";
            return Query<Banners>(sql,new {userType,pageType}); 
        }

    }
}
