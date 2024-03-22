using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;

namespace Core.Business.Services.Abstract {
    public interface IUserService
    {

        ActionMessageResponse AuthenticateUser(string userName, string password);
        Task<ActionMessageResponse> Userlogin(string userName, string password);
       Task<ActionMessageResponse> RegisterNewUser(UserRequest obj);
       
        Task<ActionMessageResponse> ChangePassword(ChangePasswordRequest model);
        Task<ActionMessageResponse> ResetUserPassword(ResetPasswordRequest model);
        Task<List<UserResponse>> UserInfo(UserSearchRequest listRequest);
        Task<UserDto> GetUserInfo(int userid, int type);



    }
}
