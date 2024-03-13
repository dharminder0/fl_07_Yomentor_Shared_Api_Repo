using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Microsoft.Owin.BuilderProperties;

namespace Core.Data.Repositories.Concrete {
    public class UserRepository : DataRepository<Users>, IUserRepository {

        public async Task UpdateLastlogin(long id) {
            var sql = $@"update Users  set  LastLoginDate = GETDATE()  where Id = @id";
            await ExecuteAsync(sql, new { id });
        }
        public Users GetUsersDetailsByToken(string accessToken) {
            var sql = $@"SELECT * FROM Users WHERE Token = @accessToken";
            return QueryFirst<Users>(sql, new { accessToken });
        }

        public IEnumerable<Users> GetUsersDetailsByUniqueCode(string uniqueCode) {
            var sql = $@"SELECT TOP 10 * FROM Users WHERE Token = @uniqueCode";
            return Query<Users>(sql, new { uniqueCode });
        }


        public IEnumerable<Users> GetUsersInfoById(int id, int brandId) {
            var sql = $@"select u.*  from Users u
            join UserBrandRef ub on u.id = ub.UserId  where BrandId = @BrandId and u.Id = @Id";
            return Query<Users>(sql, new { id, brandId });
        }


        public int VerifyUserByUsername(string phone) {
            var sql = $@"IF  EXISTS(SELECT 1 from Users where phone = @phone)
        begin
        SELECT Id from Users where phone = @phone;
        end";
            var id = ExecuteScalar<int>(sql, new { phone });
            return id;
        }


        public int InsertUser(UserRequest ob, string password, string passwordSalt) {
            if (string.IsNullOrWhiteSpace(ob.Gender)){
                ob.Gender = null;
            }

            if (ob != null && ob.DateOfBirth > DateTime.MinValue && ob.DateOfBirth < DateTime.MaxValue) {
                ob.DateOfBirth = ob.DateOfBirth?.Date;
            }

            var sql = $@"IF NOT EXISTS (SELECT 1 FROM users WHERE phone = @phone)
BEGIN
    INSERT INTO Users
           (
            
              FirstName
             ,LastName        
             ,Password
             ,PasswordSalt
             ,Token
             ,Email
             ,Phone
             ,Address
             ,type
             ,DateOfBirth
             ,Gender
             ,parentId
             ,createdate 
             ,isdeleted
            )
     VALUES
           (
        
            @FirstName
            ,@LastName
            ,@password
            ,@passwordSalt
            ,@Token
            ,@Email
            ,@Phone
            ,@Address
            ,@type
            ,@DateOfBirth
            ,@Gender
            ,@parentId
            ,GETDATE()
            ,0
            );

    SELECT SCOPE_IDENTITY() 
END
ELSE 
BEGIN
    SELECT Id FROM Users WHERE phone = @phone;
END

";
            ob.Firstname = ob.Firstname.Trim();
            return ExecuteScalar<int>(sql, new {
           
                FirstName = ob.Firstname,
                LastName = ob.Lastname,
                password = password,
                passwordSalt = passwordSalt,
                Token = Guid.NewGuid(),
                Email = ob.Email,
                Phone = ob.Phone,
               Type = ob.Type,  
                DateOfBirth = ob.DateOfBirth,
                Gender = ob.Gender,
                parentId=ob.Parentid,
                Address=ob.Address,
            });
        }




        public IEnumerable<Users> GetUsersInfoByUserName(string phone) {
            var sql = $@"
            select * from Users  where  phone=@phone  ";
            return Query<Users>(sql, new { phone });
        }



        public async Task<bool> UpdatePassword(string password, string phone) {
            var sql = $@"update users set Password =@password
 where phone = @phone ";
            return await ExecuteScalarAsync<int>(sql, new { password, phone }) > 0;
        }

        

        public async Task<IEnumerable<Users>> UserInfoVerification(string phone, string userToken) {
            var sql = $@"select  * from  Users    where phone = @phone and Token = @userToken  ";
            return await QueryAsync<Users>(sql, new { phone, userToken });
        }
        public IEnumerable<Users> GetStudentUser(List<int> studentId)
        {
            var sql = "SELECT * FROM Users WHERE Id IN @StudentIds AND type = '3'";
            return Query<Users>(sql, new { StudentIds = studentId });
        }




    }
}


