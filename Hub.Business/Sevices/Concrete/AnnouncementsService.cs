using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly IAnnoucementsRepository _annoucementsRepository;
        public AnnouncementsService(IAnnoucementsRepository annoucementsRepository)
        {
            _annoucementsRepository = annoucementsRepository;
        }

        public async Task<List<Announcements>> GetAnnouncement(int teacherId)
        {
            if(teacherId <= 0)
            {
                throw new Exception("it cannot be null");
            }
            try
            {
                var res= await _annoucementsRepository.GetAnnouncement(teacherId);
                List<Announcements> announcementsList = new List<Announcements>();
                foreach (var item in res) { 
                Announcements announcements = new Announcements();
                    announcements.Id = item.Id;
                    announcements.TeacherId= item.TeacherId;
                    announcements.BatchId= item.BatchId;
                    announcements.Announcement= item.Announcement;
                    announcements.CreateOn= item.CreateOn;
                    announcements.UpdateOn= item.UpdateOn;
                    announcementsList.Add(announcements);
                }
                return announcementsList;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
