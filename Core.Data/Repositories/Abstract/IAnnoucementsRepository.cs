using Core.Business.Entities.DataModels;
using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract
{
    public interface IAnnoucementsRepository :IDataRepository<Announcements>
    {
        Task<IEnumerable<Announcements>> GetAnnouncement(int teacherId);
    }
}
