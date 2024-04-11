using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract {
    public interface IBookService {
        Task<ActionMessageResponse> UpsertBook(BookRequest book);
        Task<ActionMessageResponse> UpsertBookExchange(BookExchange book);
        BooksResponse GetBooksList(int bookId);
        bool UpdateStatus(int id, int status);   
        Task<List<BookExchangeResponse>> GetBookExchangeList(BookExchangeRequest bookExchange);
        Task<List<BookResponseV2>> GetBooks(BookRequestV2 book);


    }
}
