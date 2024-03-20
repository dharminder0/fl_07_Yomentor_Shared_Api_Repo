using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;

namespace Core.Data.Repositories.Abstract {
    public interface IUserRepository
    {
        Task UpdateLastlogin(long id);
        Users GetUsersDetailsByToken(string accessToken);
        IEnumerable<Users> GetUsersDetailsByUniqueCode(string uniqueCode);
        IEnumerable<Users> GetUsersInfoById(int id,int brandId);
        int InsertUser(UserRequest ob, string password, string passwordSalt);
        IEnumerable<Users> GetUsersInfoByUserName(string userName);
        Task<bool> UpdatePassword(string password, string userName);
   
        Task<IEnumerable<Users>> UserInfoVerification(string phone, string userToken);
        
        int VerifyUserByUsername(string userName);
        IEnumerable<Users> GetStudentUser(List<int> studentId);
        Task<Users> GetUser(int Id);
        Task<IEnumerable<Users>> UserInfo(UserSearchRequest listRequest);



        }
 
}
