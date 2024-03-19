using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class ReviewsRepository :DataRepository<Reviews>,IReviewsRepository
    {
        public async Task<int> InsertReviews(Reviews reviews)
        {
            var sql = @"
        IF NOT EXISTS (SELECT 1 FROM reviews WHERE Id = @Id)
        BEGIN
            INSERT INTO Reviews
            (
                Addedby,
                Addedfor,
                batchid,
                rating,
                review,
                createdate,
                updatedate,
                isdeleted
            )
            VALUES
            (
                @Addedby,
               @Addedfor,
               @batchid,
               @rating,
               @review,
               @createdate,
               '',
               '0'
            );

            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SELECT Id FROM reviews WHERE Id = @Id;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, reviews);
        }

        public async Task<int> UpdateReviews(Reviews reviews)
        {
            var sql = @"
        IF EXISTS (SELECT 1 FROM reviews WHERE Id = @Id)
        BEGIN
            UPDATE reviews
            SET
                Addedby=@addedby,
                Addedfor=@addedfor,
                batchid=@batchid,
                rating=@rating,
                review=@review,
                updatedate=GetDate(),
                isdeleted=@isdeleted
            WHERE
                Id = @Id;

            SELECT Id FROM reviews WHERE Id = @Id;
        END
        ELSE
        BEGIN
            SELECT -1;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, reviews);
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewResponse(ReviewRequest reviewRequest)
        {
            var sql = @"SELECT 
    ut.Id AS UserId,
    a.AddedBy AS AddedByUserId,
    ut.FirstName AS AddedByFirstName,
    ut.LastName AS AddedByLastName,
    a.BatchId,
    ba.name AS BatchTitle,
    a.Rating,
    a.Review,
    a.CreateDate,
    a.UpdateDate
FROM 
    Users ut
LEFT JOIN 
    Reviews a ON a.AddedFor = ut.Id
LEFT JOIN 
    Batch ba ON ba.Id = a.BatchId
WHERE 
    ut.Id = @UserId ";
            if (reviewRequest.BatchId > 0)
            {
                sql += $@"and ba.Id=@batchId;";
            }
            if (reviewRequest.PageIndex > 0 && reviewRequest.PageIndex > 0)
            {
                sql += $@" ORDER BY Id DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }
            return await QueryAsync<ReviewResponse>(sql, reviewRequest);
        }

    }
}
