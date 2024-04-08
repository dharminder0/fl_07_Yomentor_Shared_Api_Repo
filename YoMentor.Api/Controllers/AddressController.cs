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
        /// <summary>
        /// /
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Upsert")]
        public IActionResult UpsertAddress(AddressRequest address) {
          var response=  _addressService.UpsertAddress(address); 
            return JsonExt(response);   

        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAddress")]
        public IActionResult GetUserAddress(int userId) {
            var response = _addressService.GetUserAddress(userId);
            return JsonExt(response);

        }
        /// <summary>
        /// /
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("StateList")]
        public IActionResult GetStateList() {
            var response = _addressService.GetState();
            return Ok(response);

        }

    }
}
