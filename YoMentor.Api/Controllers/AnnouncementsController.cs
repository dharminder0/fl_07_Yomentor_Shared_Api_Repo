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
  

        [HttpPost]
        [Route("GetAnnouncementList/teacherId")]
        public async Task<IActionResult> Get(AnnouncementsRequest announcements)
        {
            var res = await _announcementsService.GetAnnouncement(announcements);
            return JsonExt(res);

        }
    }
}
