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

namespace Core.Business.Sevices.Concrete {
    public class BookService: IBookService {
        private readonly  IBookRepository _book;
        private readonly IAddressRepository _address;

        public BookService(IBookRepository book, IAddressRepository address)
        {
            _book = book;  
            _address = address; 
                
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
                    res.UserAddress = address;

                }
                books.Add(res);
            }
        return books;   
        }
        public List<State> GetState() {
            return _address.GetStateList().ToList(); 
        }
    }
}
