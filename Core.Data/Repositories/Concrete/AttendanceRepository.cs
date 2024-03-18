﻿using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class AttendanceRepository : DataRepository<Attendance>, IAttendanceRepository
    {

        public async Task<int> InsertAttendance(Attendance attendance)
        {
            var sql = $@"IF NOT EXISTS (SELECT 1 FROM [dbo].[attendance] WHERE studentid = @studentid and batchid=@batchId and date=@date and status=@status)
            BEGIN
                     INSERT INTO [dbo].[attendance] (
                                        studentid,
                                        batchId,
                                        date,
                                        status,
                                        createdate
                                         )
                     VALUES (
                                        @studentid,
                                        @batchId,
                                        @date,
                                        @status,
                                        @createdate
                                        );
  SELECT SCOPE_IDENTITY() 
END
ELSE 
BEGIN
    SELECT Id FROM [dbo].[attendance] WHERE studentid = @studentid and batchId=@batchId and date=@date and status=@status ;
END ;";
           var res= ExecuteScalar<int>(sql, attendance);
            return res;
        }
        public async Task<int> UpdateAttendance(Attendance attendance)
        {

            var sql = @"
        IF EXISTS (SELECT 1 FROM [dbo].[attendance] WHERE Id = @Id)
        BEGIN
            UPDATE [dbo].[attendance]
            SET
                 studentid= @studentid,
                   batchId=@batchId,
                  date=@date,
                  status=@status,
                  createdate=@createdate,
                  updatedate=GetDate()
                 WHERE
                Id = @Id;

            SELECT Id FROM [dbo].[attendance] WHERE Id = @Id;
        END
        ELSE
        BEGIN
        
            SELECT -1;
        END;";

            return await ExecuteScalarAsync<int>(sql, attendance);
        }

        public  async Task<IEnumerable<Attendance>> GetStudentsAttendance(AttendanceRequest attendanceRequest) {


            var  sql = $@"  select * from attendance where batchid=@batchid ";
            if(attendanceRequest.StudentId > 0) {
                sql += " and StudentId=@StudentId  ";
            }
            if (!string.IsNullOrWhiteSpace(attendanceRequest.fromDate) && !string.IsNullOrWhiteSpace(attendanceRequest.fromDate)) {

                sql += $@" and  Date between '{attendanceRequest.fromDate}' and '{attendanceRequest.ToDate}' ";
            }

            if (attendanceRequest.PageIndex > 0 && attendanceRequest.PageSize > 0) {
                sql += $@" ORDER BY id  DESC
                 OFFSET(@PageSize * (@PageIndex - 1)) ROWS FETCH NEXT @PageSize ROWS ONLY; ";

            }
            return await QueryAsync<Attendance>(sql,attendanceRequest);
        }


    }
}
