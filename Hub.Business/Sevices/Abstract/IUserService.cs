using Core.Business.Entites.DataModels;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using static Core.Business.Entities.DTOs.Enum;

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
        Task<ActionMessageResponse> UpsertTeacherProfile(TeacherProfileRequest profileRequest);
        int PushNotifications(NotificationType type, int userId, int Id);
        List<PushNotifications> GetPushNotifications(int pazeSize, int pazeInedx, int userId);
        int GetPushNotificationCount(int userId);



    }
}
