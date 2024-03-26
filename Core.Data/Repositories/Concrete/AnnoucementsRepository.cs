using Core.Business.Entities.DataModels;
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
        public async Task<IEnumerable<Announcements>> GetAnnouncement(int teacherId)
        {
            var sql = $@"Select * from Announcements where teacherId=@teacherId";
            return await QueryAsync<Announcements>(sql, new { teacherId });
        }
    }
}
