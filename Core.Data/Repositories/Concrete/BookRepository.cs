using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class BookRepository : DataRepository<Books>,IBookRepository {
        public async Task<int> InsertBook(Books book) {
            var sql = @"
        INSERT INTO Books
        (
            Title,
            Author,
            ISBN,
            Genre,
            Publication_Year,
            GradeId,
            Available,
            ImageUrl,
            UserId,
            CreateDate,
            UpdateDate,
            IsDeleted
        )
        VALUES
        (
            @Title,
            @Author,
            @ISBN,
            @Genre,
            @PublicationYear,
            @GradeId,
            @Available,
            @ImageUrl,
            @UserId,
            GetUtcDate(),
            GetUtcDate(),
            @IsDeleted
        );

        SELECT SCOPE_IDENTITY();
    ";

            return await ExecuteScalarAsync<int>(sql, book);
        }

        public async Task<int> UpdateBook(Books book) {
            var sql = @"
        UPDATE Books
        SET
            Title = @Title,
            Author = @Author,
            ISBN = @ISBN,
            Genre = @Genre,
            PublicationYear = @PublicationYear,
            GradeId = @GradeId,
            Available = @Available,
            ImageUrl = @ImageUrl,
            UserId = @UserId,
            UpdateDate = GetUtcDate(),
            IsDeleted = @IsDeleted
        WHERE
            Id = @Id;

        SELECT Id FROM Books WHERE Id = @Id;
    ";

            return await ExecuteScalarAsync<int>(sql, book);
        }

        public async Task<int> InsertBookExchange(BookExchange exchange) {
            var sql = @"
        INSERT INTO Book_Exchange
        (
            SenderId,
            ReceiverId,
            BookId,
            CreateDate,
            Status
        )
        VALUES
        (
            @SenderId,
            @ReceiverId,
            @BookId,
            GetUtcDate(),
            @Status
        );

        SELECT SCOPE_IDENTITY();
    ";

            return await ExecuteScalarAsync<int>(sql, exchange);
        }

        public async Task<int> UpdateBookExchange(BookExchange exchange) {
            var sql = @"
        UPDATE Book_Exchange
        SET
            SenderId = @SenderId,
            ReceiverId = @ReceiverId,
            BookId = @BookId,
            CreateDate = GetUtcDate(),
            Status = @Status
        WHERE
            Id = @Id;

        SELECT Id FROM BookExchange WHERE Id = @Id;
    ";

            return await ExecuteScalarAsync<int>(sql, exchange);
        }
       public async Task<IEnumerable<Books>> GetBooksList() {
            var sql = @"select * from books where Available=1 and  IsDeleted=1 ";
            return await  QueryAsync<Books>(sql);
        }

    }
}
