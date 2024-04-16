using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Hub.Web.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YoMentor.Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SkillTestController : BaseApiController {
        private readonly ISkillTestService _skillTestService;
        
        public SkillTestController(ISkillTestService skillTestService)
        {
            _skillTestService = skillTestService;      
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="skillTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public async Task<IActionResult> GetSkillTestList(SkillTestRequest skillTest) {
            var response= await _skillTestService.GetSkillTestList(skillTest);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SkillTest/Id")]
        public IActionResult GetSkillTest(int id) {
            var response =  _skillTestService.GetSkillTest(id);
            return JsonExt(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attempt"></param>
        /// <returns></returns>
         [HttpPost]
        [Route("UpsertAttempt")]
        public IActionResult UpsertAttempt(Attempt attempt) {
            var response =  _skillTestService.UpsertAttempt(attempt);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="skillTestId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("QuestionsAnswers")]
        public  async Task<IActionResult> GetQuizQuestionsWithAnswers(int skillTestId) {
            var response =  await _skillTestService.GetQuizQuestionsWithAnswers(skillTestId);
            return JsonExt(response);
        }
    }
}
