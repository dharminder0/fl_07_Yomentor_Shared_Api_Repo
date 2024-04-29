using Core.Business.Entites.DataModels;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Microsoft.Owin.BuilderProperties;
using static Core.Business.Entities.DTOs.Enum;

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


        


        public int VerifyUserByUsername(string phone) {
            var sql = $@"IF  EXISTS(SELECT 1 from Users where phone = @phone)
        begin
        SELECT Id from Users where phone = @phone;
        end";
            var id = ExecuteScalar<int>(sql, new { phone });
            return id;
        }


        public int UpsertUser(UserRequest ob, string password, string passwordSalt) {
            if (string.IsNullOrWhiteSpace(ob.Gender)) {
                ob.Gender = null;
            }

            if (ob != null && ob.DateOfBirth > DateTime.MinValue && ob.DateOfBirth < DateTime.MaxValue) {
                ob.DateOfBirth = ob.DateOfBirth?.Date;
            }

            var sql = @"
        IF EXISTS (SELECT 1 FROM Users WHERE Phone = @Phone)
        BEGIN
            UPDATE Users
            SET
                FirstName = @FirstName,
                LastName = @LastName,
                Password = @Password,
                PasswordSalt = @PasswordSalt,
                Token = @Token,
                Email = @Email,
                Type = @Type,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                ParentId = @ParentId,
                Rank = @Rank,
                CreateDate = GETDATE(),
                IsDeleted = 0
            WHERE Phone = @Phone;

            SELECT Id FROM Users WHERE Phone = @Phone;
        END
        ELSE
        BEGIN
            INSERT INTO Users
            (
                FirstName,
                LastName,
                Password,
                PasswordSalt,
                Token,
                Email,
                Phone,
                Type,
                DateOfBirth,
                Gender,
                ParentId,
                CreateDate,
                IsDeleted,
                Rank
            )
            VALUES
            (
                @FirstName,
                @LastName,
                @Password,
                @PasswordSalt,
                @Token,
                @Email,
                @Phone,
                @Type,
                @DateOfBirth,
                @Gender,
                @ParentId,
                GETDATE(),
                0,
                @Rank
            );

            SELECT SCOPE_IDENTITY();
        END
    ";

            ob.FirstName = ob.FirstName.Trim();
            return ExecuteScalar<int>(sql, new {
                FirstName = ob.FirstName,
                LastName = ob.LastName,
                Password = password,
                PasswordSalt = passwordSalt,
                Token = Guid.NewGuid(),
                Email = ob.Email,
                Phone = ob.Phone,
                Type = ob.Type,
                DateOfBirth = ob.DateOfBirth,
                Gender = ob.Gender,
                ParentId = ob.Parentid,
           
                Rank = ob.Rank
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
        public async Task<Users> GetUser(int Id) {
            var sql = @" select * from users where id=@Id";
            return await QueryFirstAsync<Users>(sql, new { Id });
        }
        public Users GetUserInfo(int Id) {
            var sql = @" select * from users where id=@Id";
            return  QueryFirst<Users>(sql, new { Id });
        }

        public async Task<IEnumerable<Users>> UserInfo(UserSearchRequest listRequest) {
            string sqlString = "";
            if (!string.IsNullOrWhiteSpace(listRequest.SearchText)) {
                listRequest.SearchText = listRequest.SearchText.ToLower();
            }

            var sql = @"
        SELECT 
u.id
,u.firstname
,u.lastname
,u.phone
,u.email
,u.type
,u.createdate
,u.updatedate
,u.lastlogindate
,u.isdeleted
,u.parentid
,u.Password
,u.PasswordSalt
,u.Token

,u.Gender
,u.DateOfBirth
,u.Rank



        FROM USERS u";

          
                if(listRequest.grade != null && listRequest.grade.Any() ||
                listRequest.subject != null && listRequest.subject.Any()) {
                sql += @"
        INNER JOIN teacher_speciality ts ON u.Id = ts.teacherid";
            }

            sql += @"
        WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(listRequest.SearchText)) {
                sql += $@"
        AND (u.Firstname LIKE '%{listRequest.SearchText}%' OR
             u.Firstname + SPACE(1) + u.Lastname LIKE '%{listRequest.SearchText}%' OR
             u.Lastname LIKE '%{listRequest.SearchText}%' OR
             u.Email LIKE '%{listRequest.SearchText}%' OR
             u.Phone = '{listRequest.SearchText}')";
            }

            if (listRequest.userType > 0) {
                sql += @"
        AND u.type = @usertype";
            }

            if (listRequest.grade != null && listRequest.grade.Any()) {
                sql += $@"
        AND ts.gradeId IN ({string.Join(",", listRequest.grade)})";
            }

            if (listRequest.subject != null && listRequest.subject.Any()) {
                sql += $@"
        AND ts.subjectId IN ({string.Join(",", listRequest.subject)})";
            }
            if (listRequest.userType == 1) {
                sqlString = @"
        ORDER BY
            CASE
                WHEN u.Rank IN (1, 2, 3,4,5,6,7,8,9) THEN 0 
                ELSE 1 
            END,
            u.Rank ASC";
            }
            else {
                sqlString = " ORDER BY u.id DESC";
            }
            if (listRequest.pageIndex > 0 && listRequest.PageSize >0) {
                sql += $@"
       {sqlString}
        OFFSET (@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY;";
            }

            return await QueryAsync<Users>(sql, listRequest);
        }


        public async Task<Users> GetUserInfo(int Id, int type) {
            var sql = @" select * from users   where id=@Id";
            if(type > 0) {
                sql += " and type=@type";
            }
            return await QueryFirstAsync<Users>(sql, new { Id,type });
        }
        public async Task<TeacherProfile> GetTeacherProfile(int userId) {
            var sql = @" select * from Teacher_Profile where teacherid=@userId ";
            return await QueryFirstAsync<TeacherProfile>(sql, new { userId });
        }
        public async Task<int> UpsertTeacherProfile(TeacherProfile teacherProfile) {
            var sql = $@"
        IF NOT EXISTS (SELECT 1 FROM teacher_profile WHERE TeacherId = @TeacherId)
        BEGIN
            INSERT INTO teacher_profile
            (
                TeacherId,
                About,
                Education,
                Experience
            )
            VALUES
            (
                @TeacherId,
                @About,
                @Education,
                @Experience
            );
            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            UPDATE teacher_profile SET
                About = @About,
                Education = @Education,
                Experience = @Experience
            WHERE TeacherId = @TeacherId;
        END";

            var parameters = new {
                TeacherId = teacherProfile.TeacherId,
                About = teacherProfile.About,
                Education = teacherProfile.Education,
                Experience = teacherProfile.Experience
            };

            return await ExecuteScalarAsync<int>(sql, parameters);
        }
        public int InsertUser(UserRequest ob, string password, string passwordSalt) {
            if (string.IsNullOrWhiteSpace(ob.Gender)) {
                ob.Gender = null;
            }

            if (ob != null && ob.DateOfBirth > DateTime.MinValue && ob.DateOfBirth < DateTime.MaxValue) {
                ob.DateOfBirth = ob.DateOfBirth?.Date;
            }

            var sql = @"
        INSERT INTO Users
        (
            FirstName,
            LastName,
            Password,
            PasswordSalt,
            Token,
            Email,
            Phone,
            Type,
            DateOfBirth,
            Gender,
            ParentId,
            CreateDate,
            IsDeleted,
            Rank,
            GradeId
        )
        VALUES
        (
            @FirstName,
            @LastName,
            @Password,
            @PasswordSalt,
            @Token,
            @Email,
            @Phone,
            @Type,
            @DateOfBirth,
            @Gender,
            @ParentId,
            GETDATE(),
            0,
            @Rank,
            @GradeId
        );

        SELECT SCOPE_IDENTITY();
    ";

            ob.FirstName = ob.FirstName.Trim();
            return ExecuteScalar<int>(sql, new {
                FirstName = ob.FirstName,
                LastName = ob.LastName,
                Password = password,
                PasswordSalt = passwordSalt,
                Token = Guid.NewGuid(),
                Email = ob.Email,
                Phone = ob.Phone,
                Type = ob.Type,
                DateOfBirth = ob.DateOfBirth,
                Gender = ob.Gender,
                ParentId = ob.Parentid,
                Rank = ob.Rank,
                GradeId=ob.GradeId,
            });
        }
        public int UpdateUser(UserRequest ob) {
            if (string.IsNullOrWhiteSpace(ob.Gender)) {
                ob.Gender = null;
            }

            if (ob != null && ob.DateOfBirth > DateTime.MinValue && ob.DateOfBirth < DateTime.MaxValue) {
                ob.DateOfBirth = ob.DateOfBirth?.Date;
            }

            var sql = @"
        UPDATE Users
        SET
            FirstName = @FirstName,
            LastName = @LastName,         
            Token = @Token,
            Email = @Email,
          
            Type = @Type,
            DateOfBirth = @DateOfBirth,
            Gender = @Gender,
            ParentId = @ParentId,
            Rank = @Rank,
            CreateDate = GETDATE(),
            IsDeleted = 0,
            GradeId=@GradeId
        WHERE id = @Id;

        SELECT Id FROM Users WHERE Phone = @Phone;
    ";

            ob.FirstName = ob.FirstName.Trim();
            return ExecuteScalar<int>(sql, new {
                FirstName = ob.FirstName,
                LastName = ob.LastName,
                Token = Guid.NewGuid(),
                Email = ob.Email,
                Phone = ob.Phone,
                Type = ob.Type,
                DateOfBirth = ob.DateOfBirth,
                Gender = ob.Gender,
                ParentId = ob.Parentid,
             
                Rank = ob.Rank,
                GradeId=ob.GradeId,
                Id=ob.Id    
            });
        }

        public int AddUserDevices(int userId, string deviceToken, DateTime CreatedDate) {
            var sql = @"
if not Exists(Select 1 from User_Devices where UserId =@UserId  and deviceToken=@deviceToken)
begin
INSERT INTO User_Devices(UserId,deviceToken,CreatedDate)
      VALUES(@UserId,@deviceToken,getutcdate())
 select SCOPE_IDENTITY()
end
else
begin
select -2 
end 
";
            return ExecuteScalar<int>(sql, new {
                userId = userId,
                deviceToken = deviceToken,
                CreatedDate = CreatedDate,


            });
        }
        public bool RemoveDevices(string deviceToken) {
            var sql = @"delete from User_Devices where deviceToken =@deviceToken  ";
            return ExecuteScalar<bool>(sql, new { deviceToken });
        }

        public async Task<UserDevices> GetUserDevices(int userId) {
            var sql = @"select * from User_Devices where userId=@userId ";
            return await QueryFirstAsync<UserDevices>(sql, new { userId });


        }
        public async Task<IEnumerable<UserDevices>> GetUserDevicesV2(int userId) {
            var sql = @"select * from User_Devices where userId=@userId ";
            return await QueryAsync<UserDevices>(sql, new { userId });


        }
        public async Task<bool> UpdateStatus(int id, Status status, int notificationType) {

            var sql = @" Update Push_Notifications set status=@status  where Id=@id ";
            return await ExecuteScalarAsync<bool>(sql, new { id, status, notificationType });

        }
        public async Task<bool> UpdateNotificationStatus(int id, NotificationStatus status) {

            var sql = @" Update Push_Notifications set Notification_Status=@status where Id=@id ";
            return await ExecuteScalarAsync<bool>(sql, new { id, status });

        }
        public async Task<IEnumerable<PushNotifications>> GetPushNotifications() {

            var sql = @" SELECT top 50 *
            FROM Push_Notifications
        ";

            return await QueryAsync<PushNotifications>(sql);



        }
    }
}


