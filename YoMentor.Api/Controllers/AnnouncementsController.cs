using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : BaseApiController
    {
        private readonly IAnnouncementsService _announcementsService;
        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;   
        }


        [HttpGet]
        [Route("GetAnnouncement/Id")]

        public async Task<IActionResult> getAnnouncements(int id)
        {
            var res =await _announcementsService.GetbyId(id);
            return JsonExt(res);
        }

        [HttpPost]
        [Route("GetAnnouncementList/teacherId")]
        public async Task<IActionResult> Get(AnnouncementsRequest announcements)
        {
            var res = await _announcementsService.GetAnnouncement(announcements);
            return JsonExt(res);

        }
        [HttpPost]
        [Route("Upsert")]
        public async Task<IActionResult> AddAnnouncements(Announcements announcements)
        {
            var res= await _announcementsService.AddAnnouncements(announcements);
            return JsonExt(res);
        }
        [HttpPost]
        [Route("GetAnnouncementList/batchId")]
        public async Task<IActionResult> Getthroughbatch(AnnouncementsRequest announcements)
        {
            var res = await _announcementsService.Getthroughbatch(announcements);
            return JsonExt(res);

        }
    }
}
