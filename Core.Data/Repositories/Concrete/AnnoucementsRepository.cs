using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class AnnoucementsRepository : DataRepository<Announcements>, IAnnoucementsRepository
    {
        public async Task<Announcements> GetById(int Id)
        {
            var sql = @"SELECT * FROM Announcements WHERE Id = @Id";
            var res = await QueryFirstAsync<Announcements>(sql,new { Id });
            return res;
        }
        public async Task<IEnumerable<Announcements>> GetAnnouncement(AnnouncementsRequest announcements)
        {
            var parameters = new DynamicParameters();

            var sql = @"SELECT * FROM Announcements WHERE 1=1";

            if (announcements.BatchId > 0)
            {
                sql += " AND BatchId = @BatchId";
                parameters.Add("@BatchId", announcements.BatchId);
            }

            if (announcements.TeacherId > 0)
            {
                sql += " AND TeacherId = @TeacherId";
                parameters.Add("@TeacherId", announcements.TeacherId);
            }

            var res = await QueryAsync<Announcements>(sql, parameters);
            return res;
        }

        public async Task<IEnumerable<Announcements>> Getthroughbatch(AnnouncementsRequest announcements)
        {
            var sql = @"SELECT * FROM Announcements WHERE 1=1";
            var parameters = new DynamicParameters(); // Using DynamicParameters for dynamic parameter assignment

            if (announcements.BatchId > 0)
            {
                sql += " AND BatchId = @BatchId";
                parameters.Add("@BatchId", announcements.BatchId); // Add BatchId parameter if provided
            }

            if (announcements.TeacherId > 0)
            {
                sql += " AND TeacherId = @TeacherId";
                parameters.Add("@TeacherId", announcements.TeacherId); // Add TeacherId parameter if provided
            }

            var res = await QueryAsync<Announcements>(sql, parameters); // Execute query with parameters
            return res;
        }

        public async Task<int> InsertAnnouncements(Announcements announcements)
        {
            var sql = @"
        IF NOT EXISTS (SELECT 1 FROM announcements WHERE Id = @Id)
        BEGIN
           INSERT INTO [dbo].[announcements]
           ([BatchId]
           ,[TeacherId]
           ,[Announcement]
           ,[CreatedOn]
           ,[UpdatedOn])
     VALUES
           (@BatchId,
           @TeacherId,
           @Announcement,
           GetDate(),
           GetDate())

            SELECT SCOPE_IDENTITY();
        END
        ELSE
        BEGIN
            SELECT Id FROM Announcements WHERE Id = @Id;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, announcements);
        }

        public async Task<int> UpdateAnnouncements(Announcements announcements)
        {
            var sql = @"
        IF EXISTS (SELECT 1 FROM announcements WHERE Id = @Id)
        BEGIN
          UPDATE [dbo].[announcements]
        SET [BatchId] = @BatchId
      ,[TeacherId] = @TeacherId
      ,[Announcement] = @Announcement
      ,[CreatedOn] = GetDate()
      ,[UpdatedOn] = GetDate()
            WHERE 
                Id = @Id;

            SELECT Id FROM Announcements WHERE Id = @Id;
        END
        ELSE
        BEGIN
            SELECT -1;
        END;
    ";

            return await ExecuteScalarAsync<int>(sql, announcements);
        }

    }
}
