using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract
{
    public interface IAnnouncementsService
    {
        Task<List<Announcements>> GetAnnouncement(AnnouncementsRequest announcements);
        Task<Announcements> GetbyId(int id);
        Task<ActionMessageResponse> AddAnnouncements(Announcements announcements);
        Task<List<Announcements>> Getthroughbatch(AnnouncementsRequest announcements);
    }
}
