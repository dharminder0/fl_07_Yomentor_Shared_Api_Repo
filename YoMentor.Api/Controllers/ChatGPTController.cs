using Azure;
using Core.Business.Entities.ChatGPT;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;
using YoMentor.ChatGPT;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : BaseApiController {
        private readonly IAIQuestionAnswerService _aIQuestionAnswer;

        public ChatGPTController(IAIQuestionAnswerService aIQuestionAnswer) {
            _aIQuestionAnswer = aIQuestionAnswer;
        }


        [HttpPost]
        [Route("GetQuestion")]
        public async Task<IActionResult> GenerateQuestions([FromBody] QuestionRequest request) {
            var questions = await _aIQuestionAnswer.GenerateQuestions(request, false);

            return Ok(questions);
        }


        [HttpPost]
        [Route("GetQuestionObject")]
        public async Task<IActionResult> GenerateQuestionsObject([FromBody] QuestionRequest request) {
            var questions = await _aIQuestionAnswer.GenerateQuestions(request, true);

            return Ok(questions);
        }
        [HttpPost("createPrompt")]
        public async Task<IActionResult> CreatePrompt([FromBody] QuestionRequest request) {

            var questions = await _aIQuestionAnswer.GenerateQuestions(request);
            return JsonExt(questions);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillTestRequest"></param>
        /// <returns></returns>
        [HttpPost("SkillTest-attempts/dailyCount")]
        public IActionResult GetAttemptCount(SkillTestRequestV2 skillTestRequest) {
            var questions =  _aIQuestionAnswer.GetAttemptCountV2(skillTestRequest.UserId,skillTestRequest.Startdate,skillTestRequest.EndDate);
            return JsonExt(questions);
        }
    }
}

