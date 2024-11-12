using Azure;
using Azure.Core;
using Core.Business.Entites.DataModels;
using Core.Business.Entites.RequestModels;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Services.Abstract;
using Core.Common.Account;
using Core.Common.Web;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Hub.Common.Settings;
using RLV.Security.Lib;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using static Core.Business.Entities.DTOs.Enum;
using Message = FirebaseAdmin.Messaging.Message;

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
        private readonly IAddressRepository _addressRepository; 
        private  readonly IPushnotificationsRepository _ushnotificationsRepository; 
        

        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository usersRepository, IReviewsRepository reviewsRepository, IMediaFileRepository mediaFileRepository, ITeacherSpecialityRepository teacherSpecialityRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository, IAddressRepository addressRepository, IPushnotificationsRepository ushnotificationsRepository) {
            _userRepository = usersRepository;
            _reviewsRepository = reviewsRepository;
            _mediaFileRepository = mediaFileRepository;
            _teacherSpecialityRepository = teacherSpecialityRepository; 
            _gradeRepository = gradeRepository; 
            _subjectRepository = subjectRepository; 
            _addressRepository = addressRepository;
            _ushnotificationsRepository = ushnotificationsRepository;   


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
          
            int userId = 0;
           

            if (obj.Id > 0) {

                userId= _userRepository.UpdateUser(obj);

                return new ActionMessageResponse { Success = false, Content = userId, Message = "Update_Suucessfully." };
            }

           
                var userinfo = _userRepository.GetUsersInfoByUserName(obj.Phone).ToList();
                if (!userinfo.Any()) {
                    var salt = Hasher.GenerateSalt();
                    var hashedPassword = Hasher.HashPassword(salt, obj.Password);
                    userId = _userRepository.InsertUser(obj, hashedPassword, salt);
                try {
                    if(obj.Type== (int)UserType.Teacher) {
                        TeacherProfile tch = new TeacherProfile();
                      
                        tch.TeacherId = userId;
                      


                        await _userRepository.UpsertTeacherProfile(tch);
                    }
                } catch (Exception) {

                   
                }
                    return new ActionMessageResponse { Success = true, Content = userId, Message = "User inserted successfully." };
                
            }
           
            return new ActionMessageResponse { Success = false, Message = "User_already_exsist" };
        }

        public ActionMessageResponse DeleteUser(int userId, bool isDeleted) {

            var res = _userRepository.DeleteUser(userId, isDeleted);
            return new ActionMessageResponse { Success = true, Message = "successfully deleted" };

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





        //public ActionMessageResponse AuthenticateUser(string phone, string password) {
        //    var user = new UserAuthenticationDto();
        //    if (!string.IsNullOrWhiteSpace(phone) && !string.IsNullOrEmpty(password)) {
        //        var dbUser = _userRepository.GetUsersInfoByUserName(phone).FirstOrDefault();
        //        if (dbUser != null && dbUser.PasswordSalt != null) {
        //            var saltBytes = dbUser.PasswordSalt;
        //            decryption password
        //            var hashedPassword = Hasher.HashPassword(saltBytes, password);
        //            if (hashedPassword == dbUser.Password) {
        //                user = MapUserToUserBasicDto(dbUser);
        //                return new ActionMessageResponse { Content = user };
        //            }
        //        }
        //        else {
        //            if (dbUser == null)
        //                user.AuthenticationStatus = false;
        //            return new ActionMessageResponse { Content = user };
        //        }
        //    }
        //    return new ActionMessageResponse { Success = false, Message = "required username and password", Content = 0 };
        //}

        private UserAuthenticationDto MapUserToUserBasicDto(Users dbUser) {
            if (dbUser.Id == 0) {
                return new UserAuthenticationDto();
            }

            var user = new UserAuthenticationDto();

            var files = _mediaFileRepository.GetEntityMediaFile(dbUser.Id, MediaEntityType.Users);
            if (files != null && files.Any()) {
                user.Image = files.First().BlobLink;
            }
            int category = dbUser.Category;

            user.Id = dbUser.Id;
            user.FirstName = dbUser.FirstName;
            user.LastName = dbUser.LastName;

            user.Phone = dbUser.Phone;
            user.Token = GenerateUserJwtEncryptedToken(dbUser.Token);
            user.ParentId = dbUser.Parentid;          
            user.Type = dbUser.Type;
            user.AuthenticationStatus = true;
            user.StudentGradeId = dbUser.GradeId;
            user.Gender= dbUser.Gender;     
            user.Email = dbUser.Email;  
            user.DateOfBirth = dbUser.DateOfBirth;  
            user.Rank = dbUser.Rank;    
            user.IsDeleted = dbUser.IsDeleted; 
            user.Category= category >0 ? category : 0;
            string categoryName = _gradeRepository.GetCategorieName(dbUser.Category);
            user.CategoryName =!string.IsNullOrEmpty(categoryName) ? categoryName : string.Empty;

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
                res.FirstName = item.FirstName;
                res.LastName = item.LastName;
                res.Phone = item.Phone;
                res.Rank = item.Rank;
                var image = _mediaFileRepository.GetImage(item.Id, MediaEntityType.Users);
                if (image != null) {
                    res.ProfilePicture = image.BlobLink;
                }
                var addressInfo = _addressRepository.GetUserAddress(item.Id);
                if (addressInfo != null) {
                    Address address = new Address();
                    address.Address1 = addressInfo.Address1;
                    address.Address2 = addressInfo.Address2;
                    address.UserId = addressInfo.UserId;
                    address.StateId = addressInfo.StateId;
                    address.Latitude = addressInfo.Latitude;
                    address.Longitude = addressInfo.Longitude;
                    address.City = addressInfo.City;
                    address.IsDeleted = addressInfo.IsDeleted;
                    address.Id = addressInfo.Id;
                    address.Pincode = addressInfo.Pincode;
                    address.UpdateDate = addressInfo.UpdateDate;
                    try {
                        var stateName = _addressRepository.GetState(address.StateId);
                        address.StateName = stateName.Name;

                    } catch (Exception) {


                    }
                    res.UserAddress = address;

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
                res.FirstName = item.FirstName;
                res.LastName = item.LastName;
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
                            var teacherSpeciality = await _teacherSpecialityRepository.GetTeacherSpeciality(userid);
                            foreach (var item in teacherSpeciality) {
                                var newGrade = new GradeSubjectResponse(); 

                                newGrade.GradeId = item.GradeId;
                                newGrade.SubjectId = item.SubjectId;
                                newGrade.GradeName = _gradeRepository.GetGradeName(item.GradeId);
                                newGrade.SubjectName = _subjectRepository.GetSubjectName(item.SubjectId);
                                

                                teacher.GradeSubjectList.Add(newGrade); 
                            }

                            userDto.TeacherSpeciality = teacher;
                        } catch (Exception) {
                            // Handle exceptions here if needed
                        }


                    }
                }
                
                var image = _mediaFileRepository.GetImage(userid, MediaEntityType.Users);
                if (image != null) {
                    userDto.Image = image.BlobLink;
                }
              var addressInfo=  _addressRepository.GetUserAddress(userid);
                if (addressInfo != null) {
                    Address address = new Address();
                    address.Address1 = addressInfo.Address1;    
                    address.Address2 = addressInfo.Address2;    
                    address.UserId = addressInfo.UserId;
                    address.StateId = addressInfo.StateId;  
                    address.Latitude = addressInfo.Latitude;    
                    address.Longitude = addressInfo.Longitude;  
                    address.City = addressInfo.City;
                    address.IsDeleted = addressInfo.IsDeleted;  
                    address.Id = addressInfo.Id;    
                    address.Pincode = addressInfo.Pincode;  
                    address.UpdateDate = addressInfo.UpdateDate;
                    try {
                        var stateName = _addressRepository.GetState(address.StateId);
                        address.StateName = stateName.Name;

                    } catch (Exception) {

                     
                    }
                  userDto.UserAddress = address;  
                   

                }
                userDto.IsDeleted = response.IsDeleted;
                userDto.UpdateDate = response.UpdateDate;
                userDto.FirstName = response.FirstName;
                userDto.LastLoginDate = response.LastLoginDate;
                userDto.Email = response.Email;
                userDto.LastName = response.LastName;
               
                userDto.Phone = response.Phone;
                userDto.DateOfBirth = response.DateOfBirth;
                userDto.ParentId = response.Parentid;
                userDto.CreateDate = response.CreateDate;
                userDto.Id = response.Id;
                userDto.Type = response.Type;
                userDto.Rank=response.Rank;
                userDto.Gender = response.Gender;
                userDto.Category = response.Category;
                string categoryName = _gradeRepository.GetCategorieName(response.Category);
                userDto.CategoryName = !string.IsNullOrEmpty(categoryName) ? categoryName : string.Empty;
             
 
                userDto.StudentGradeId = response.GradeId;
           
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
        public int  PushNotifications(NotificationType type, int userId, int Id) {
            PushNotificationRequest pushNotifications = new PushNotificationRequest();
            var notification = _ushnotificationsRepository.pushNotificationType(type.ToString());

        
            pushNotifications.NotificationMessage = notification.Message;
            pushNotifications.Notificationtitle = notification.Title;
            pushNotifications.NotificationTypeId = notification.Id;
  
            pushNotifications.NotificationDateTime = DateTime.UtcNow.AddMinutes(5);

         int id=   _ushnotificationsRepository.AddPushNotifications((int)type, userId, pushNotifications.Notificationtitle, pushNotifications.NotificationMessage, pushNotifications.NotificationDateTime, DateTime.Now, (int)Status.Pending, (int)NotificationStatus.NotSent, Id, false);

            return id;
        }
        public List<PushNotifications> GetPushNotifications(int pazeSize, int pazeInedx, int userId) {
            var response = _ushnotificationsRepository.GetPushNotifications(pazeSize, pazeInedx, userId);
            List<PushNotifications> app = new List<PushNotifications>();
            foreach (var item in response) {

                PushNotifications push = new PushNotifications();
                push.Id = item.Id;
                push.UserId = userId;
                push.NotificationTypeId = item.NotificationTypeId;
                push.Status = item.Status;
                push.NotificationStatus = item.NotificationStatus;
                push.NotificationTitle = item.NotificationTitle;
                push.NotificationMessage = item.NotificationMessage;
                push.CreatedDate = item.CreatedDate;
                push.IsRead = item.IsRead;
                push.EntityId = item.EntityId;
                push.NotificationDateTime = item.NotificationDateTime;
                push.NotificationTypeId = item.NotificationTypeId;
              
                app.Add(push);


            }
            return app;


        }
        public int GetPushNotificationCount(int userId) {
            return _ushnotificationsRepository.GetPushNotificationsCount(userId);


        }
        public ActionMessageResponse RemoveUsers(string deviceToekn) {

            var res = _userRepository.RemoveDevices(deviceToekn);
            return new ActionMessageResponse { Success = true, Message = "successfully deleted" };

        }
        public ActionMessageResponse AddUserDevices(UserDevicesRequest deviceRefRequest) {

            try {

                //   _userRepository.RemoveDevices(deviceRefRequest.DeviceToken.ToString());
                if (deviceRefRequest != null && deviceRefRequest.UserId.Any())
                    foreach (var item in deviceRefRequest.UserId) {

                        _userRepository.AddUserDevices(item, deviceRefRequest.DeviceToken, deviceRefRequest.CreatedDate);

                    }
                return new ActionMessageResponse { Message = "Assigned successfully ", Success = true, Content = true };

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> GetPushNotification() {
            PushNotifications pushNotifications = new PushNotifications();
            var response = await _userRepository.GetPushNotifications();

            foreach (var notification in response) {
                try {
                    var deviceDetailsList = await _userRepository.GetUserDevicesV2(notification.UserId);
                    if (deviceDetailsList != null && deviceDetailsList.Any()) {
                        foreach (var deviceDetails in deviceDetailsList) {
                            try {
                                if (!string.IsNullOrWhiteSpace(deviceDetails.DeviceToken)) {
                                    await SendPushNotificationAsync(deviceDetails.DeviceToken, notification.NotificationTitle, notification.NotificationMessage);
                                    await _userRepository.UpdateStatus(notification.Id, Status.Delivered, notification.NotificationTypeId);
                                    await _userRepository.UpdateNotificationStatus(notification.Id, NotificationStatus.Sent);
                                }
                            } catch (Exception ex) {
                                await _userRepository.UpdateStatus(notification.Id, Status.Failed, 0);
                            }
                        }
                    }
                    else {
                        await _userRepository.UpdateStatus(notification.Id, Status.DeviceNotFound, 0);
                    }
                } catch (Exception) {
                    await _userRepository.UpdateStatus(notification.Id, Status.Failed, 0);
                }
            }
            return true;
        }
        public async Task SendPushNotificationAsync(string registrationToken, string title, string body) {
            try {
                var message = new Message {
                    Token = registrationToken,
                    Notification = new Notification {
                        Title = title,
                        Body = body
                    }
                };

                var messaging = FirebaseMessaging.DefaultInstance;


                await messaging.SendAsync(message);
            } catch (FirebaseException ex) {

                throw;
            }
        }
    }
}