using Core.Business.Entities.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Abstract
{
    public interface IAnnouncementsService
    {
        Task<List<Announcements>> GetAnnouncement(int teacherId);
    }
}
