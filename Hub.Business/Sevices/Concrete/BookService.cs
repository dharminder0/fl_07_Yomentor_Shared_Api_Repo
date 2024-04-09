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
    public class BookService: IBookService {
        private readonly  IBookRepository _book;
        private readonly IAddressRepository _address;
        private readonly IUserRepository _user; 

        public BookService(IBookRepository book, IAddressRepository address, IUserRepository user)
        {
            _book = book;  
            _address = address; 
            _user=user;
                
        }
        public async Task<ActionMessageResponse> UpsertBook(BookRequest book) {
            int res = 0;
            if (book == null) { return new ActionMessageResponse { Success = false }; }
            Books book1=new Books();  
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

            if (book.Id == 0) {
               res= await  _book.InsertBook(book1);
                return new ActionMessageResponse { Success = true,Content=res,Message=" Insertion_Successfully" };

            }
          res=  await   _book.UpdateBook(book1);
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
        public async Task<List<BooksResponse>> GetBooksList() {
         
         var bookInfo=  await  _book.GetBooksList();
            List <BooksResponse> books = new List<BooksResponse>();  
            foreach (var item in bookInfo) {
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
                res.ImageUrl = item.ImageUrl;
                var addressInfo = _address.GetUserAddress(item.UserId);
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
                        var stateName = _address.GetState(address.StateId);
                        address.StateName = stateName.Name;

                    } catch (Exception) {


                    }
                    res.UserAddress = address;

                }
                books.Add(res);
            }
        return books;   
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


    }
}
