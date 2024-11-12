using Core.Business.Entites.DataModels;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Data.Repositories.Abstract {
    public interface IUserRepository
    {
        Task UpdateLastlogin(long id);
        Users GetUsersDetailsByToken(string accessToken);
        IEnumerable<Users> GetUsersDetailsByUniqueCode(string uniqueCode);

      
        IEnumerable<Users> GetUsersInfoByUserName(string userName);
        Task<bool> UpdatePassword(string password, string userName);
   
        Task<IEnumerable<Users>> UserInfoVerification(string phone, string userToken);
        
        int VerifyUserByUsername(string userName);
        IEnumerable<Users> GetStudentUser(List<int> studentId);
        Task<Users> GetUser(int Id);
        Task<IEnumerable<Users>> UserInfo(UserSearchRequest listRequest);
         Task<Users> GetUserInfo(int Id, int type);
        Task<TeacherProfile> GetTeacherProfile(int userId);
        Task<int> UpsertTeacherProfile(TeacherProfile teacherProfile);
        int UpdateUser(UserRequest ob);
        int InsertUser(UserRequest ob, string password, string passwordSalt);
        Users GetUserInfo(int Id);
        int AddUserDevices(int userId, string deviceToken, DateTime CreatedDate);
        bool RemoveDevices(string deviceToken);
        Task<UserDevices> GetUserDevices(int userId);
        Task<IEnumerable<UserDevices>> GetUserDevicesV2(int userId);
        Task<bool> UpdateStatus(int id, Status status, int notificationType);
        Task<bool> UpdateNotificationStatus(int id, NotificationStatus status);
        Task<IEnumerable<PushNotifications>> GetPushNotifications();
        bool DeleteUser(int userId, bool isDeleted);






        }
 
}
