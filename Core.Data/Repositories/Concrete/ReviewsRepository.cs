﻿using Core.Business.Entities.DataModels;
using Core.Business.Entities.DTOs;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections;
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
            var sql = @"SELECT A.Id, 
    a.AddedFor AS AddedForUserId,
    a.AddedBy AS AddedByUserId,
    ut.FirstName AS AddedForFirstName,
    ut.LastName AS AddedForLastName,
    uty.FirstName As AddedByFirstName,
    uty.LastName As AddedByLastName,
	a.BatchId,
    a.Rating,
    a.Review,
    a.CreateDate,
    a.UpdateDate
    FROM 
    Reviews a 
    JOIN 
   Users ut ON  a.AddedFor = ut.Id
Join
Users uty On a.Addedby= uty.Id";
 sql+=@"
WHERE a.AddedFor = @AddedFor ";
            if(reviewRequest.AddedBy >0) {
                 sql += $@" and a.Addedby=@Addedby"; 
            }
            if (reviewRequest.BatchId > 0)
            {
                if (reviewRequest.AddedBy > 0 || reviewRequest.AddedBy > 0) { sql += $@" and a.batchId=@batchId;"; }
                else { sql += $@"WHERE a.batchId=@batchId;"; }
            }
            if (reviewRequest.PageIndex > 0 && reviewRequest.PageIndex > 0)
            {
                sql += $@" ORDER BY a.Id ASC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }
           var res= await QueryAsync<ReviewResponse>(sql, reviewRequest);
            return res;
        }

        public async Task<IEnumerable<Reviews>> GetReviewsForTeacher(int teacherId) {
            var sql = @" select * from reviews where Addedfor=@teacherId ";
            return await QueryAsync<Reviews>(sql, new { teacherId });
        }
    }
}
