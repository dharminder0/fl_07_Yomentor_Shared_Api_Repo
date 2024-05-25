﻿using Core.Business.Entities.ChatGPT;
using Core.Business.Sevices.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using YoMentor.ChatGPT;

namespace YoMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTController : ControllerBase
    {
        private readonly IAIQuestionAnswerService _aIQuestionAnswer;

        public ChatGPTController(IAIQuestionAnswerService aIQuestionAnswer) {
            _aIQuestionAnswer = aIQuestionAnswer;
        }


        [HttpPost]
        [Route("GetQuestion")]
        public async Task<IActionResult> GenerateQuestions([FromBody] QuestionRequest request)
        {
            var questions =await _aIQuestionAnswer.GenerateQuestions(request,false);

            return Ok(questions);
        }


        [HttpPost]
        [Route("GetQuestionObject")]
        public async Task<IActionResult> GenerateQuestionsObject([FromBody] QuestionRequest request)
        {
            var questions = await _aIQuestionAnswer.GenerateQuestions(request,true);

            return Ok(questions);
        }
    }
}

