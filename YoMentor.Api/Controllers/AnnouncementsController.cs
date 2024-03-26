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
        [Route("{teacherId}")]
        public async Task<IActionResult> Get(int teacherId)
        {
            var res = await _announcementsService.GetAnnouncement(teacherId);
            return JsonExt(res);

        }
    }
}
