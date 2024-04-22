using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Dapper;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

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
            UserId,
            CreateDate,
            UpdateDate,
            Remark,
            IsDeleted,
            SubjectId 
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
            @UserId,
            GetUtcDate(),
            GetUtcDate(),
            @Remark,
            @IsDeleted
            ,@SubjectId
            
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
            Publication_Year = @PublicationYear,
            GradeId = @GradeId,
            Available = @Available,
            UserId = @UserId,
            UpdateDate = GetUtcDate(),
            Remark=@Remark,
            IsDeleted = @IsDeleted,
            SubjectId=@SubjectId
        WHERE
            Id = @Id;

        SELECT Id FROM Books WHERE Id = @Id;
    ";

            return await ExecuteScalarAsync<int>(sql, book);
        }

        public async Task<int> InsertBookExchange(BookExchange exchange) {
            var sqlCheck = @"
        SELECT COUNT(*) 
        FROM Book_Exchange 
        WHERE receiverId = @receiverId 
        AND BookId = @BookId;
    ";

            var count = await ExecuteScalarAsync<int>(sqlCheck, exchange);

            if (count > 0) {

              return  await  UpdateBookExchange(exchange);
            }

            var sqlInsert = @"
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

            return await ExecuteScalarAsync<int>(sqlInsert, exchange);
        }


        public async Task<int> UpdateBookExchange(BookExchange exchange) {
            var sql = @"
        UPDATE Book_Exchange
        SET           
            Status = @Status
       WHERE receiverId = @receiverId 
        AND BookId = @BookId;

        SELECT Id FROM Book_Exchange WHERE Id = @Id;
    ";

            return await ExecuteScalarAsync<int>(sql, exchange);
        }
       public Books GetBooksList(int bookId) {
            var sql = @"select * from books where Available=1 and  IsDeleted=0 and id=@bookid ";
            return   QueryFirst<Books>(sql,new { bookId });
        }
        public  bool  UpdateStatus(int id, int status, int receiverId) {
            var sql = @" update Book_Exchange set status=@status where BookId=@id and receiverid=@receiverId    ";
            return ExecuteScalar<bool>(sql,new { id, status ,receiverId}); 

        }
  
        public string GetBookName(int id) {
            var sql = @" select Title from books where id=@id ";
            return QueryFirst<string>(sql,new { id }); 
        }
        public async Task<IEnumerable<BookExchange>> GetBooks(BookExchangeRequest bookExchange) {
            var sql = @"
        SELECT b.*, ";

            if (bookExchange.SenderId > 0 || bookExchange.ReceiverId > 0 || bookExchange.StatusId.Any()) {
                sql += @"
            be.id AS ExchangeId, be.senderid AS SenderId, be.receiverid AS ReceiverId,
            be.status AS ExchangeStatus, be.createdate AS ExchangeCreateDate,
            be.updatedate AS ExchangeUpdateDate, be.isdeleted AS ExchangeIsDeleted,
        ";
            }

            sql += @"
        FROM Books b ";

            if (bookExchange.SenderId > 0 || bookExchange.ReceiverId > 0 || bookExchange.StatusId.Any()) {
                sql += @"
            LEFT JOIN Book_Exchange be ON b.id = be.bookid
            WHERE 1=1 ";
            }

            if (bookExchange.SenderId > 0) {
                sql += " AND be.senderid = @SenderId ";
            }
            if (bookExchange.ReceiverId > 0) {
                sql += " AND be.receiverid = @ReceiverId ";
            }
            if (bookExchange.StatusId.Any()) {
                sql += " AND be.status IN @StatusId ";
            }

            if (bookExchange.PageIndex > 0 && bookExchange.PageSize > 0) {
                sql += @"
            ORDER BY b.id
            OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";
            }

            return await QueryAsync<BookExchange>(sql, bookExchange);
        }

     
        public async Task<IEnumerable<BookResponseV2>> GetBookList(BookRequestV2 book) {
            var status = "";
            var parameters = new DynamicParameters();

            if (book.ActionType==(int) BookActionType.IsRequested) {
                status = ",be.status,be.createdate as  RequestedDate";
            }

            var sql = $@"
        SELECT DISTINCT b.*{status}
        FROM books b
    ";

            if (book.UserId > 0 && book.ActionType==(int)BookActionType.IsRequested) {
                sql += @"
            JOIN book_Exchange be ON b.id = be.bookId
            WHERE be.status = 1 AND be.receiverid = @UserId
        ";
                parameters.Add("@UserId", book.UserId); 
            }
            else {
                sql += @"
            WHERE 1=1
            
        ";
                if (book.ActionType == 0) {
                    sql += @" and UserId not in(select UserId from books where UserId=@UserId) ";
                    parameters.Add("@UserId", book.UserId);
                }
            }
            sql += " and b.available= 1 and b.isdeleted= 0 ";

            if (book.UserId > 0 && book.ActionType == (int)BookActionType.IsCreated) {
                sql += @" AND b.userid = @UserId ";
                parameters.Add("@UserId", book.UserId); 
            }

            if (book.GradeId > 0) {
                sql += @" AND b.GradeId = @GradeId ";
                parameters.Add("@GradeId", book.GradeId);
            }

            if (!string.IsNullOrWhiteSpace(book.SearchText)) {
                sql += @"
            AND (b.Title LIKE @SearchText OR b.Author LIKE @SearchText)
        ";
                parameters.Add("@SearchText", $"%{book.SearchText}%");
            }

            if (book.PageIndex > 0 && book.PageSize > 0) {
                sql += @"
            ORDER BY b.id
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

                parameters.Add("@PageSize", book.PageSize);
                parameters.Add("@Offset", book.PageSize * (book.PageIndex - 1));
            }

            return await QueryAsync<BookResponseV2>(sql, parameters);
        }

        public int GetStatusName(int id,int bookId) {
            var sql = @" select status  from book_exchange where receiverid=@id and bookid=@bookId ";
            return QueryFirst<int >(sql,  new { id ,bookId}); 
        }
        public bool UpdateBookStatus(int id) {
            var sql = @" update books set available= 0  where id=@id";
            return ExecuteScalar<bool>(sql, new { id });

        }
        public int GetStatusNameV2(int id, int bookId) {
            var sql = @" select status  from book_exchange where senderid=@id and bookid=@bookId  ";
            return QueryFirst<int>(sql, new { id, bookId });
        }
        public bool DeleteBook(int id) {
            var sql = @" update books set isdeleted= 1  where id=@id";
            return ExecuteScalar<bool>(sql, new { id });


        }
        public DateTime  GetRequestedDate(int bookId) {
            var sql = @" select createdate from Book_Exchange where bookId=@bookId ";
            return ExecuteScalar<DateTime>(sql, new { bookId });
        }
        public IEnumerable<int>  GetReciverId(int bookId, int senderId) {
            var sql = @" select receiverid from  Book_Exchange where bookId=@bookId  and senderid=@senderId 
 ";
            return Query<int>(sql, new { bookId, senderId });
        }
        public IEnumerable<int> GetReciverIdV2(int bookId) {
            var sql = @" select receiverid from  Book_Exchange where bookId=@bookId  and status=1
 ";
            return Query<int>(sql, new { bookId });
        }
    }
}
