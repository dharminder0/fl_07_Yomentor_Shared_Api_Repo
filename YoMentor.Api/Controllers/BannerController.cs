using Autofac.Core;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : BaseApiController {
        private readonly IBannersService _BannersService;
        public BannersController(IBannersService BannersService)
        {
            _BannersService = BannersService; 
                
        }
        [HttpGet]
        [Route("GetBnners")]
        public IActionResult GetBannerss(int userType,int pageType ) {
            var response = _BannersService.GetBannerss(pageType,userType);
            return JsonExt(response);
        }
    }
}
