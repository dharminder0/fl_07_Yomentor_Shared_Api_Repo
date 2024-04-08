using Core.Business.Entities.DataModels;
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
        Task<IEnumerable<Books>> GetBooksList();
    }
}
