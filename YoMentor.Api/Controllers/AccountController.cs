using Core.Business.Entities.RequestModels;
using Core.Business.Services.Abstract;
using Hub.Web.Api.Controllers;
using Hub.Web.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.API.Controllers {
    //[Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController {
        private readonly IUserService _userService;
        public AccountController(IUserService usersService) {
            _userService = usersService;
        }


        /// <summary>
        /// used to authenticate user 
        /// </summary>
        /// <param name="credentials">object contains user name and password</param>
        /// <remarks> 
        /// </remarks>
        [HttpPost]
        [Route("api/User/login")]


        public async Task<IActionResult> Userlogin(AuthenticationRequest credentials) {
            try {
                var user = await _userService.Userlogin(credentials.Phone, credentials.Password);
                return JsonExt(user);
            } catch (Exception ex) {

                return JsonExt(ex.Message);
            }
        }



        /// <summary>
        /// Sets a new password for the user given the current one
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/change-password")]
        //[Route("change-password")]

        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model) {
            var result = await _userService.ChangePassword(model);
            return JsonExt(result);
        }



        /// <summary>
        /// Sets a new password through email link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/reset-password")]
        //[Route("reset-password")]

        public async Task<IActionResult> ResetUserPassword(ResetPasswordRequest model) {
            try {
                var result = await _userService.ResetUserPassword(model);
                return JsonExt(result);
            } catch (Exception ex) {
                return JsonExt(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/Upsert")]

        public async Task<IActionResult> AddUser(UserRequest obj) {
            try {

                var response = await _userService.RegisterNewUser(obj);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex.Message);


            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/UserInfo")]
        public async Task<IActionResult> UserInfo(UserSearchRequest listRequest) {
            try {

                var response = await _userService.UserInfo(listRequest);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex.Message);


            }
        }

    }
    
}
  
