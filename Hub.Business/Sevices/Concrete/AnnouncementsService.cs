using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
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

        public async Task<Announcements> GetbyId(int id)
        {
            if(id <= 0)
            {
                return null;
            }
            try
            {
                var res= await _annoucementsRepository.GetById(id);
                return res;
            }
            catch(Exception ex)
            {
                throw;
            }
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
        public async Task<List<Announcements>> Getthroughbatch(AnnouncementsRequest announcements)
        {
            if (announcements == null)
            {
                throw new Exception("it cannot be null");
            }
            try
            {
                var res = await _annoucementsRepository.Getthroughbatch(announcements);
                List<Announcements> announcementsList = new List<Announcements>();
                foreach (var item in res)
                {
                    Announcements announcementsdata = new Announcements();
                    announcementsdata.Id = item.Id;
                    announcementsdata.TeacherId = item.TeacherId;
                    announcementsdata.BatchId = item.BatchId;
                    announcementsdata.Announcement = item.Announcement;
                    announcementsdata.CreateOn = item.CreateOn;
                    announcementsdata.UpdateOn = item.UpdateOn;
                    announcementsList.Add(announcementsdata);
                }
                return announcementsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ActionMessageResponse>AddAnnouncements(Announcements announcements)
        {
            if (announcements == null)
            {
               return new ActionMessageResponse { Success=false, Content="null",Message="Data is provided by you is empty" };

            }
            try
            {
                if (announcements.Id == 0)
                {
                    int insertedId = await _annoucementsRepository.InsertAnnouncements(announcements);
                    return new ActionMessageResponse { Content = insertedId, Message = "announcements_created", Success = true };
                }

                int id = await _annoucementsRepository.UpdateAnnouncements(announcements);
                return new ActionMessageResponse { Content = id, Message = "announcements_Updated", Success = true };

            }
            catch (Exception ex)
            {
                return new ActionMessageResponse { Success = false, Content = ex.Message, Message = "Error Occur" };
            }

        }
    }
}
