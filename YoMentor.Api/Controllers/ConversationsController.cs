using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : BaseApiController {
        private readonly IConversationService _conversationService;
        public ConversationsController(IConversationService conversationService)
        {
            _conversationService= conversationService;  
        }
        [HttpPost]
        [Route("UpsertConversation")]
        public IActionResult UpsertConversation(Conversation conversation) {
            var  response=_conversationService.UpsertConversation(conversation);
            return JsonExt(response);
        }
        [HttpPost]
        [Route("UpsertMessages")]
        public IActionResult UpsertMessage(Conversations_Messages msg) {
            var response = _conversationService.UpsertMessage(msg);
            return JsonExt(response);
        }
    }
}
