using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
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
        Task<IEnumerable<Announcements>> GetAnnouncement(AnnouncementsRequest announcements);
        Task<Announcements> GetById(int Id);
        Task<int> InsertAnnouncements(Announcements announcements);
        Task<int> UpdateAnnouncements(Announcements announcements);
        Task<IEnumerable<Announcements>> Getthroughbatch(AnnouncementsRequest announcements);
    }
}
