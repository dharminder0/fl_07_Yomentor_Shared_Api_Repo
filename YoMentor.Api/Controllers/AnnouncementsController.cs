using Core.Business.Sevices.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementsService _announcementsService;
        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;   
        }

     
    }
}
