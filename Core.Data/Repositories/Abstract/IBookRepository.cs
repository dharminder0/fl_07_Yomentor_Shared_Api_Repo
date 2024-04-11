using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IBookRepository  : IDataRepository<Books>{
        Task<int> InsertBook(Books book);
        Task<int> UpdateBook(Books book);
        Task<int> InsertBookExchange(BookExchange exchange);
        Task<int> UpdateBookExchange(BookExchange exchange);
        Books GetBooksList(int bookId);
        bool UpdateStatus(int id, int status);
        Task<IEnumerable<BookExchange>> GetBooks(BookExchangeRequest bookExchange);
        string GetBookName(int id);
        Task<IEnumerable<BookResponseV2>> GetBookList(BookRequestV2 book);
        int  GetStatusName(int id,int bookId);
    }
}
