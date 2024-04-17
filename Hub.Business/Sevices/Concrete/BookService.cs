using AutoMapper.Execution;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.Dto;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Sevices.Concrete {
    public class BookService : IBookService {
        private readonly IBookRepository _book;
        private readonly IAddressRepository _address;
        private readonly IUserRepository _user;
        private readonly IGradeRepository _grade;
        private readonly IMediaFileRepository _mediaFile;
        private readonly ISubjectRepository _subject;

        public BookService(IBookRepository book, IAddressRepository address, IUserRepository user, IGradeRepository grade, IMediaFileRepository mediaFile, ISubjectRepository subject) {
            _book = book;
            _address = address;
            _user = user;
            _grade = grade;
            _mediaFile = mediaFile;
            _subject = subject;
        }
        public async Task<ActionMessageResponse> UpsertBook(BookRequest book) {
            int res = 0;
            if (book == null) { return new ActionMessageResponse { Success = false }; }
            Books book1 = new Books();
            book1.Id = book.Id;
            book1.Title = book.Title;
            book1.ISBN = book.ISBN;
            book1.Author = book.Author;
            book1.UpdateDate = book.UpdateDate;
            book1.CreateDate = book.CreateDate;
            book1.IsDeleted = book.IsDeleted;
            book1.Genre = book.Genre;
            book1.Available = book.Available;
            book1.GradeId = book.GradeId;
            book1.UserId = book.UserId;
            book1.PublicationYear = book.PublicationYear;
            book1.Remark = book.Remark;
            book1.SubjectId = book.SubjectId;

            if (book.Id == 0) {
                res = await _book.InsertBook(book1);
                return new ActionMessageResponse { Success = true, Content = res, Message = " Insertion_Successfully" };

            }
            res = await _book.UpdateBook(book1);
            return new ActionMessageResponse { Success = true, Content = res, Message = " Update_Successfully" };
        }
        public async Task<ActionMessageResponse> UpsertBookExchange(BookExchange book) {
            int res = 0;
            if (book == null) { return new ActionMessageResponse { Success = false }; }



            if (book.Id == 0) {
                res = await _book.InsertBookExchange(book);
                return new ActionMessageResponse { Success = true, Content = res, Message = " Insertion_Successfully" };

            }
            res = await _book.UpdateBookExchange(book);
            return new ActionMessageResponse { Success = true, Content = res, Message = " Update_Successfully" };
        }
        public BooksResponse GetBooksList(int bookId) {

            var item = _book.GetBooksList(bookId);
            if (item == null) { return null; }

            UserBasic user = new UserBasic();
            BooksResponse res = new BooksResponse();
            res.Title = item.Title;
            res.ISBN = item.ISBN;
            res.UpdateDate = item.UpdateDate;
            res.CreateDate = item.CreateDate;
            res.IsDeleted = item.IsDeleted;
            res.Available = item.Available;
            res.Genre = item.Genre;
            res.Author = item.Author;
            res.Id = item.Id;
            res.UserId = item.UserId;
            res.PublicationYear = item.PublicationYear;
            res.GradeId = item.GradeId;
            res.SubjectId = item.SubjectId;
            string subjectname = _subject.GetSubjectName(res.SubjectId);
            if (!string.IsNullOrEmpty(subjectname)) {
                res.SubjectName = subjectname;
            }
            DateTime requestDate = _book.GetRequestedDate(bookId);
            if (requestDate != DateTime.MinValue) {
                res.RequestedDate = requestDate;
            }
            string gradeName = _grade.GetGradeName(res.GradeId);
            if (!string.IsNullOrEmpty(gradeName)) {
                res.GradeName = gradeName;
            }
            try {
                var Image = _mediaFile.GetImage(item.Id, MediaEntityType.Book);
                if (Image != null) {
                    res.ImageUrl = Image.BlobLink;
                }

            } catch (Exception) {


            }

            int stusId = _book.GetStatusNameV2(item.UserId, item.Id);
            if (stusId > 0) {
                res.Status = stusId;
            }
            string status = Enum.GetName(typeof(BookExchangeStatus), res.Status);
            if (!string.IsNullOrEmpty(status)) {
                res.StatusName = status;
            }
            res.Remark = item.Remark;

            var userInfo = _user.GetUserInfo(item.UserId);
            if (userInfo != null) {
                var image = _mediaFile.GetImage(item.UserId, MediaEntityType.Users);



                user.FirstName = userInfo.FirstName;
                user.LastName = userInfo.LastName;
                user.Email = userInfo.Email;
                user.Phone = userInfo.Phone;
                if (image != null) {
                    user.UserImage = image.BlobLink;
                }


            }


            var addressInfo = _address.GetUserAddress(item.UserId);
            if (addressInfo != null) {
                var address = new Address {
                    Address1 = addressInfo.Address1,
                    Address2 = addressInfo.Address2,
                    UserId = addressInfo.UserId,
                    StateId = addressInfo.StateId,
                    Latitude = addressInfo.Latitude,
                    Longitude = addressInfo.Longitude,
                    City = addressInfo.City,
                    IsDeleted = addressInfo.IsDeleted,
                    Id = addressInfo.Id,
                    Pincode = addressInfo.Pincode,
                    UpdateDate = addressInfo.UpdateDate

                };
                var stateName = _address.GetState(address.StateId);
                if (stateName != null) {
                    address.StateName = stateName.Name;
                }
                user.UserAddress = address;


            }
            res.UserInfo = user;
            return res; 
        }


    
            
            

        
       

        public bool UpdateStatus(int id, int status) {
            return _book.UpdateStatus(id, status);  
        }
        public async Task<List<BookExchangeResponse>> GetBookExchangeList(BookExchangeRequest bookExchange) {
            if (bookExchange == null) {
                return null;
            }

            var bookExchangObj = await _book.GetBooks(bookExchange);
            var res = new List<BookExchangeResponse>();

            foreach (var book in bookExchangObj) {
                var obj = new BookExchangeResponse {
                    Id = book.Id,
                    SenderId = book.SenderId,
                    ReceiverId = book.ReceiverId,
                    StatusId = book.Status,
                    CreatedDate = book.CreateDate,
                    BookId = book.BookId,
                    BookName = _book.GetBookName(book.BookId)
                };

                await PopulateUserInfo(obj, bookExchange.SenderId);
                await PopulateUserInfo(obj, bookExchange.ReceiverId);

                obj.StatusName = Enum.GetName(typeof(BookExchangeStatus), book.Status);
                res.Add(obj);
            }

            return res;
        }

        private async Task PopulateUserInfo(BookExchangeResponse obj, int userId) {
            if (userId <= 0) return;

            var userInfo = await _user.GetUser(userId);
            if (userInfo == null) return;

            var user = new UserBasic {
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Email = userInfo.Email,
                Phone = userInfo.Phone
            };

            var addressInfo = _address.GetUserAddress(userId);
            if (addressInfo != null) {
                var address = new Address {
                    Address1 = addressInfo.Address1,
                    Address2 = addressInfo.Address2,
                    UserId = addressInfo.UserId,
                    StateId = addressInfo.StateId,
                    Latitude = addressInfo.Latitude,
                    Longitude = addressInfo.Longitude,
                    City = addressInfo.City,
                    IsDeleted = addressInfo.IsDeleted,
                    Id = addressInfo.Id,
                    Pincode = addressInfo.Pincode,
                    UpdateDate = addressInfo.UpdateDate
                };
                user.UserAddress = address;
            }

            obj.UserInfo = user;
        }
        public async Task< List<BookResponseV2>> GetBooks(BookRequestV2 book) {
            List<BookResponseV2> res = new List<BookResponseV2>();
            var rws = await  _book.GetBookList(book);
            foreach (var item in rws) {
                BookResponseV2 obj=new BookResponseV2();
                obj.UserId = item.UserId;
                obj.Author = item.Author; 
                obj.Title = item.Title; 
                obj.Id=item.Id;
                obj.PublicationYear=item.PublicationYear;
                obj.GradeId = item.GradeId;
                obj.CreateDate = item.CreateDate; 
                obj.SubjectId=item.SubjectId;
                obj.Available=item.Available;   
                obj.RequestedDate=item.RequestedDate;   
               string subjectName= _subject.GetSubjectName(obj.SubjectId);
                if (!string.IsNullOrEmpty(subjectName)) {
                    obj.SubjectName = subjectName;  
                }
                if (book.ActionType !=(int)BookActionType.IsRequested) {
                    int stusId = _book.GetStatusName(book.UserId, obj.Id);
                    if (stusId > 0) {
                        obj.Status = stusId;
                    }
                }
                else {
                    obj.Status = item.Status;
                }
                string grade= _grade.GetGradeName(item.GradeId);
                if (!string.IsNullOrEmpty(grade)) {
                    obj.GradeName = grade;  
                }
                try {
                   var Image= _mediaFile.GetImage(item.Id, MediaEntityType.Book);
                    if (Image != null) {
                        obj.ImageUrl = Image.BlobLink;
                    }

                } catch (Exception) {

                   
                }
                string status = Enum.GetName(typeof(BookExchangeStatus), obj.Status);
                if(!string.IsNullOrEmpty(status)) {
                    obj.StatusName = status;
                }
                res.Add(obj);
            }
         return res;    
        }
        public bool UpdateBookStatus(int id) {
            return _book.UpdateBookStatus(id);
        }
        public bool DeleteBook(int id) {
            return _book.DeleteBook(id);
        }
    }
}
