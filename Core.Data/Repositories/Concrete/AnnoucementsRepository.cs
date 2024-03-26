using Core.Business.Entities.DataModels;
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
            var sql = @"SELECT * FROM Announcements WHERE TeacherId = @TeacherId";
            object parameters = new { TeacherId = announcements.TeacherId };
            if (announcements.BatchId >= 0)
            {
                sql += " AND BatchId = @BatchId";
                parameters = new { TeacherId = announcements.TeacherId, BatchId = announcements.BatchId };
            }
            var res = await QueryAsync<Announcements>(sql, parameters);
            return res;
        }


    }
}
