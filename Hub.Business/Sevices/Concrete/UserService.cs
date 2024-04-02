using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Services.Abstract;
using Core.Common.Account;
using Core.Common.Web;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using Hub.Common.Settings;
using RLV.Security.Lib;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Services.Concrete {
    public class UserService : IUserService {
        protected HttpService _httpService;
        private static bool _userTokenEncryptionEnabled = GlobalSettings.IsUserTokenEncryptionEnabled;
        private static string _symmetricSecretKey = GlobalSettings.BlobSymmetricSecretKey;
        private static string _jwtIssuer = GlobalSettings.JwtIssuer;
        private static string _jwtAudience = GlobalSettings.JwtAudience;
        private readonly IReviewsRepository _reviewsRepository;
        private readonly IMediaFileRepository _mediaFileRepository;
        private readonly ITeacherSpecialityRepository _teacherSpecialityRepository;
        private readonly IGradeRepository _gradeRepository; 
        private readonly ISubjectRepository _subjectRepository; 
        

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository usersRepository, IReviewsRepository reviewsRepository, IMediaFileRepository mediaFileRepository, ITeacherSpecialityRepository teacherSpecialityRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository) {
            _userRepository = usersRepository;
            _reviewsRepository = reviewsRepository;
            _mediaFileRepository = mediaFileRepository;
            _teacherSpecialityRepository = teacherSpecialityRepository; 
            _gradeRepository = gradeRepository; 
            _subjectRepository = subjectRepository; 


        }

        public async Task<ActionMessageResponse> Userlogin(string userName, string password) {
            var user = new UserAuthenticationDto();
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrEmpty(password)) {
                var dbUser = _userRepository.GetUsersInfoByUserName(userName).FirstOrDefault();
                if (dbUser != null && dbUser.PasswordSalt != null) {
                    var saltBytes = dbUser.PasswordSalt;

                    var hashedPassword = Hasher.HashPassword(saltBytes, password);
                    if (hashedPassword == dbUser.Password) {
                        user = MapUserToUserBasicDto(dbUser);                       
                        if (user.AuthenticationStatus == true) {
                            await _userRepository.UpdateLastlogin(user.Id);
                            return new ActionMessageResponse { Success = true, Message = "Successfully_Login", Content = user };
                        }
                    }

                    return new ActionMessageResponse { Success = false, Message = "invalid_username_password" };
                }
                else {
                    if (dbUser == null)
                        user.AuthenticationStatus = false;
                    return new ActionMessageResponse { Success = false, Message = "invalid_username_password", Content = 0 };
                }
            }
            return new ActionMessageResponse { Success = false, Message = "required username and password", Content = 0 };
        }



        public async Task<ActionMessageResponse> RegisterNewUser(UserRequest obj) {
            var salt = Hasher.GenerateSalt();
            var hashedPassword = Hasher.HashPassword(salt, obj.Password);
            int userId = 0;
           

            if (obj.Id > 0) {

                userId= _userRepository.UpdateUser(obj, hashedPassword, salt);

                return new ActionMessageResponse { Success = false, Content = userId, Message = "User alreadyExsist." };
            }

            if (obj.Id == 0) {
               
                userId = _userRepository.InsertUser(obj, hashedPassword, salt);
                return new ActionMessageResponse { Success = true, Content = userId, Message = "User inserted successfully." };
            }

            return new ActionMessageResponse { Success = false, Message = "Failed to register user." };
        }

   

        public async Task<ActionMessageResponse> ChangePassword(ChangePasswordRequest model) {
            try {
                if (model == null && string.IsNullOrWhiteSpace(model.Phone) && string.IsNullOrWhiteSpace(model.CurrentPassword) && string.IsNullOrWhiteSpace(model.NewPassword)) {
                    return new ActionMessageResponse { Success = false, Message = "Required Parameter missing" };
                }

                var user = _userRepository.GetUsersInfoByUserName(model.Phone).FirstOrDefault();
                if (user != null) {
                    var saltBytes = user.PasswordSalt;
                    var oldHashedPassword = Hasher.HashPassword(saltBytes, model.CurrentPassword);
                    if (user.Password == oldHashedPassword) {
                        user.Password = Hasher.HashPassword(saltBytes, model.NewPassword);
                        await _userRepository.UpdatePassword(user.Password, user.Email);
                        return new ActionMessageResponse {
                            Success = true,
                            Message = "Password Change Successfully "
                        };
                    }
                    else {
                        return new ActionMessageResponse {
                            Success = false,
                            Message = "Old password is incorrect"
                        };
                    }
                }
                else {
                    return new ActionMessageResponse {
                        Success = false,
                        Message = "User is not found"
                    };
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ActionMessageResponse> ResetUserPassword(ResetPasswordRequest model) {
            try {
                if (model != null && !string.IsNullOrWhiteSpace(model.Phone) && !string.IsNullOrWhiteSpace(model.NewPassword) && !string.IsNullOrWhiteSpace(model.UserToken)) {
                    string Phone = model.Phone;
                    string newPassword = model.NewPassword;
                    string userToken = model.UserToken;
                    var userList = await _userRepository.UserInfoVerification(Phone, userToken);
                    var user = userList.FirstOrDefault();
                    if (user != null) {
                        var saltBytes = user.PasswordSalt;
                        if (!string.IsNullOrWhiteSpace(saltBytes)) {
                            user.Password = Hasher.HashPassword(saltBytes, newPassword);
                            await _userRepository.UpdatePassword(user.Password, user.Email);
                            return new ActionMessageResponse {
                                Success = true,
                                Message = "Password Change Successfully "
                            };
                        }
                    }
                    else {
                        return new ActionMessageResponse {
                            Success = false,
                            Message = "User is not found"
                        };
                    }
                }
                return new ActionMessageResponse {
                    Success = false,
                    Message = "Required Parameter"
                };
            } catch (Exception ex) {

                throw new Exception(ex.Message);
            }
        }





        public ActionMessageResponse AuthenticateUser(string phone, string password) {
            var user = new UserAuthenticationDto();
            if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrEmpty(password)) {
                var dbUser = _userRepository.GetUsersInfoByUserName(phone).FirstOrDefault();
                if (dbUser != null && dbUser.PasswordSalt != null) {
                    var saltBytes = dbUser.PasswordSalt;
                    // decryption password
                    var hashedPassword = Hasher.HashPassword(saltBytes, password);
                    if (hashedPassword == dbUser.Password) {
                        user = MapUserToUserBasicDto(dbUser);
                        return new ActionMessageResponse { Content = user };
                    }
                }
                else {
                    if (dbUser == null)
                        user.AuthenticationStatus = false;
                    return new ActionMessageResponse { Content = user };
                }
            }
            return new ActionMessageResponse { Success = false, Message = "required username and password", Content = 0 };
        }

        private UserAuthenticationDto MapUserToUserBasicDto(Users dbUser) {
            if (dbUser.Id == 0) {
                return new UserAuthenticationDto();
            }

            var user = new UserAuthenticationDto();

            var files = _mediaFileRepository.GetEntityMediaFile(dbUser.Id, MediaEntityType.Users);
            if (files != null && files.Any()) {
                user.Image = files.First().BlobLink;
            }
            user.Id = dbUser.Id;
            user.Firstname = dbUser.Firstname;
            user.Lastname = dbUser.Lastname;

            user.Phone = dbUser.Phone;
            user.Token = GenerateUserJwtEncryptedToken(dbUser.Token);
            user.Parentid = dbUser.Parentid;
            user.Address = dbUser.Address;
            user.Type = dbUser.Type;
            user.AuthenticationStatus = true;

            return user;
        }




        private static string GenerateUserJwtEncryptedToken(string accessToken) {
            var encryptionEnabled = _userTokenEncryptionEnabled;
            var symmetricSecretKey = _symmetricSecretKey;
            var issuer = _jwtIssuer;
            var audience = _jwtAudience;
            var jwtExpirationMinutes = 120;
            if (encryptionEnabled) {
                return JwtSecurityService.Encrypt(symmetricSecretKey, JwtSecurityService.BuildJwtToken(symmetricSecretKey, accessToken, issuer, audience, jwtExpirationMinutes));
            }
            else {
                return accessToken;
            }
        }

        public async Task<List<UserResponse>> UserInfo(UserSearchRequest listRequest) {
            var response = await _userRepository.UserInfo(listRequest);
            List<UserResponse> responses = new List<UserResponse>();





            foreach (var item in response) {
                if (item.Rank == 0) {
                    item.Rank = 999;

                }
                UserResponse res = new UserResponse();
                res.Id = item.Id;
                res.FirstName = item.Firstname;
                res.LastName = item.Lastname;
                res.Phone = item.Phone;
                res.Rank = item.Rank;
                var image = _mediaFileRepository.GetImage(item.Id, MediaEntityType.Users);
                if (image != null) {
                    res.ProfilePicture = image.BlobLink;
                }
                if (item.Type == (int)UserType.Teacher) {

                    var review = await _reviewsRepository.GetReviewsForTeacher(item.Id);
                    if (review != null && review.Any()) {
                        int rating = review.Select(x => x.Rating).Sum();
                        int average = rating / review.Select(v => v.Rating).Count();
                        res.AverageRating = average;
                        res.ReviewCount = review.Select(v => v.Rating).Count();
                    }
                    var teacherInfo = await _userRepository.GetTeacherProfile(item.Id);
                    if (teacherInfo != null) {
                        res.Experience = teacherInfo.Experience;
                        res.About = teacherInfo.About;
                        res.Education = teacherInfo.Education;

                    }
                }
                responses.Add(res);

            }
            if (listRequest.userType == (int)UserType.Teacher) {
                responses = responses.OrderBy(r => r.Rank)
                             .ThenByDescending(r => r.AverageRating)
                             .ToList();
            }
            foreach (var item in responses) {
                if (item.Rank == 999) {
                    item.Rank = 0;
                }

            }

            return responses;
        }
        public async Task<List<UserResponse>> UserInfo2(UserSearchRequest listRequest) {
            var response = await _userRepository.UserInfo(listRequest);
            List<UserResponse> responses = new List<UserResponse>();

            foreach (var item in response) {
                UserResponse res = new UserResponse();
                res.Id = item.Id;
                res.FirstName = item.Firstname;
                res.LastName = item.Lastname;
                res.Phone = item.Phone;
                res.Rank = item.Rank;

                var image = _mediaFileRepository.GetImage(item.Id, MediaEntityType.Users);
                if (image != null) {
                    res.ProfilePicture = image.BlobLink;
                }

                if (item.Type == (int)UserType.Teacher) {
                    var review = await _reviewsRepository.GetReviewsForTeacher(item.Id);
                    if (review != null && review.Any()) {
                        int rating = review.Select(x => x.Rating).Sum();
                        int average = review.Where(v => v.Rating != 0).Select(v => v.Rating).Count() > 0 ?
                            rating / review.Where(v => v.Rating != 0).Select(v => v.Rating).Count() : 0;
                        res.AverageRating = average;
                        res.ReviewCount = review.Select(v => v.Rating).Count();
                    }
                    var teacherInfo = await _userRepository.GetTeacherProfile(item.Id);
                    if (teacherInfo != null) {
                        res.Experience = teacherInfo.Experience;
                        res.About = teacherInfo.About;
                        res.Education = teacherInfo.Education;
                    }
                }
                responses.Add(res);
            }

            if (listRequest.userType == (int)UserType.Teacher) {
                responses = responses.Where(r => r.Rank != 0 || r.AverageRating != 0)
                    .OrderByDescending(r => r.Rank != 0)
                    .ThenBy(r => r.Rank)
                    .ThenByDescending(r => r.AverageRating != 0)
                    .ThenByDescending(r => r.AverageRating)
                    .ToList();
            }

            return responses;
        }

        public async Task<UserDto> GetUserInfo(int userid, int type) {
            UserDto userDto = new UserDto();
             
            TeacherSpecialityResponse teacher = new TeacherSpecialityResponse();
            teacher.GradeSubjectList = new List<GradeSubjectResponse>();
            GradeSubjectResponse grade = new GradeSubjectResponse();
            List<GradeSubjectResponse> GradeSubjectList = null;
            var response = await _userRepository.GetUserInfo(userid, type);




            if (response != null) {
                if (response.Type == (int)UserType.Teacher) {
                    var teacherInfo = await _userRepository.GetTeacherProfile(response.Id);
                    if (teacherInfo != null) {

                        TeacherProfileResponse teacherobj = new TeacherProfileResponse();
                        teacherobj.Experience = teacherInfo.Experience;
                        teacherobj.About = teacherInfo.About;
                        teacherobj.Education = teacherInfo.Education;
                        teacherobj.TeacherId=teacherInfo.TeacherId;    
                        userDto.TeacherProfile= teacherobj;
                        try {
                            var teacherrSpeciality= await   _teacherSpecialityRepository.GetTeacherSpeciality(userid);
                            foreach (var item in teacherrSpeciality) {
                                
                               
                                teacher.TeacherId = item.TeacherId;
                               
                                grade.GradeId = item.GradeId;
                                grade.SubjectId=item.SubjectId;
                                grade.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                                grade.SubjectName = _subjectRepository.GetSubjectName(item.SubjectId);
                                teacher.TeacherId=item.TeacherId;


                                teacher.GradeSubjectList.Add(grade);


                            }
                               

                            userDto.TeacherSpeciality = teacher;

                        } catch (Exception) {

                           
                        }

                    }
                }
                var image = _mediaFileRepository.GetImage(userid, MediaEntityType.Users);
                if (image != null) {
                    userDto.Image = image.BlobLink;
                }

                userDto.IsDeleted = response.IsDeleted;
                userDto.UpdateDate = response.UpdateDate;
                userDto.Firstname = response.Firstname;
                userDto.LastLoginDate = response.LastLoginDate;
                userDto.Email = response.Email;
                userDto.Lastname = response.Lastname;
                userDto.Address = response.Address;
                userDto.Phone = response.Phone;
                userDto.DateOfBirth = response.DateOfBirth;
                userDto.Parentid = response.Parentid;
                userDto.CreateDate = response.CreateDate;
                userDto.Id = response.Id;
                userDto.Type = response.Type;
                userDto.Address = response.Address;
                userDto.StudentGradeId = response.GradeId;
                userDto.DateOfBirth = response.DateOfBirth;
                if (userDto.Type == (int)UserType.Teacher) {
                    var review = await _reviewsRepository.GetReviewsForTeacher(userDto.Id);
                    if (review != null && review.Any()) {
                        int rating = review.Select(x => x.Rating).Sum();
                        int average = review.Where(v => v.Rating != 0).Select(v => v.Rating).Count() > 0 ?
                            rating / review.Where(v => v.Rating != 0).Select(v => v.Rating).Count() : 0;
                        userDto.AverageRating = average;
                        userDto.ReviewCount = review.Select(v => v.Rating).Count();
                    }

                    return userDto;



                }
               

            }
            return userDto;
        }
        public  async Task<ActionMessageResponse> UpsertTeacherProfile(TeacherProfileRequest profileRequest) {
            if (profileRequest == null) {
                return new ActionMessageResponse { Success = false };

            }
            TeacherProfile teacherProfile = new TeacherProfile();
            teacherProfile.About = profileRequest.About;    
            teacherProfile.Education= profileRequest.Education; 
            teacherProfile.TeacherId= profileRequest.TeacherId; 
            teacherProfile.Experience= profileRequest.Experience;
            int  id= await  _userRepository.UpsertTeacherProfile(teacherProfile);
            return new ActionMessageResponse { Success = true,Content=id };    
        }
    }
}