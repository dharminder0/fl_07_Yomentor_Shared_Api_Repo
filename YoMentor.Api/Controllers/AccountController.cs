using Core.Business.Entities.RequestModels;
using Core.Business.Services.Abstract;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Hub.Web.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web.API.Controllers {
    //[Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController {
        private readonly IUserService _userService;
        private readonly ITeacherSpecialityService _teacherSpeciality;
        public AccountController(IUserService usersService, ITeacherSpecialityService teacherSpeciality) {
            _userService = usersService;
            _teacherSpeciality = teacherSpeciality; 
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
        [Route("api/User/UserSearch")]
        public async Task<IActionResult> UserInfo(UserSearchRequest listRequest) {
            try {

                var response = await _userService.UserInfo(listRequest);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex.Message);


            }
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/UserDetails")]
        public async Task<IActionResult> GetUserInfo(int userid, int type) {
            var response = await _userService.GetUserInfo(userid, type);
            return JsonExt(response);   
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/UpsertTeacherSpeciality")]
        public async Task<IActionResult> AssignTeacherSpeciality(TeacherSpecialityRequest request) {
            var response = await _teacherSpeciality.TeacherSpeciality(request);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/User/ProfileUpsert")]
        public async Task<IActionResult> UpsertTeacherProfile(TeacherProfileRequest obj) {
            try {

                var response = await _userService.UpsertTeacherProfile(obj);
                return JsonExt(response);
            } catch (Exception ex) {
                return JsonExt(ex.Message);


            }
        }
    }
    
}
  
