using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Business.Sevices.Concrete;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : BaseApiController { 
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertAddress(AddressRequest address) {
          var response=  _addressService.UpsertAddress(address); 
            return JsonExt(response);   

        }

    }
}
