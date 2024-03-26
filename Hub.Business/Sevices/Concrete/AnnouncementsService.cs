using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
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

        public async Task<List<Announcements>> GetAnnouncement(AnnouncementsRequest announcements)
        {
            if(announcements== null)
            {
                throw new Exception("it cannot be null");
            }
            try
            {
                var res= await _annoucementsRepository.GetAnnouncement(announcements);
                List<Announcements> announcementsList = new List<Announcements>();
                foreach (var item in res) { 
                Announcements announcementsdata = new Announcements();
                    announcementsdata.Id = item.Id;
                    announcementsdata.TeacherId= item.TeacherId;
                    announcementsdata.BatchId= item.BatchId;
                    announcementsdata.Announcement= item.Announcement;
                    announcementsdata.CreateOn= item.CreateOn;
                    announcementsdata.UpdateOn= item.UpdateOn;
                    announcementsList.Add(announcementsdata);
                }
                return announcementsList;
            }
            catch(Exception ex)
            {
               throw new Exception(ex.Message);
            }
        }
    }
}
