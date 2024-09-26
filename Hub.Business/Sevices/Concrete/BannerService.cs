using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class BannersService:IBannersService {
        private readonly IBannersRepository _BannersRepository;
        public BannersService(IBannersRepository BannersRepository)
        {
            _BannersRepository = BannersRepository; 
        }
        public  List<Banners> GetBannerss(int pageType, int userType) {
            var response= _BannersRepository.GetBanners(userType, pageType).ToList();
            return response;
        }
    }
}
