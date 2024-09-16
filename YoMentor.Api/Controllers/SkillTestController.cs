using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
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
        public IActionResult GetSkillTest(int id,int userId) {
            var response =  _skillTestService.GetSkillTest(id,userId);
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
        public  async Task<IActionResult> GetQuizQuestionsWithAnswers(int skillTestId, int attemptId) {
            var response =  await _skillTestService.GetQuizQuestionsWithAnswers(skillTestId, attemptId);
            return JsonExt(response);
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="skillTestId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BulkAttemptDetail")]
        public IActionResult AttemptDetailBulkInsert(SkillTestAttemptRequest skillTestId) {
            var response =  _skillTestService.AttemptDetailBulkInsert(skillTestId);
            return JsonExt(response);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="skillTest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ListByUser")]
        public async Task<IActionResult> GetSkillTestListByUser(SkillTestRequest skillTest) {
            var response = await _skillTestService.GetSkillTestListByUser(skillTest);
            return JsonExt(response);
        }
        [HttpPost]
        [Route("SimilerSkillTestList")]
        public async Task<IActionResult> GetSimilerSkillTestList(SkillTestRequest skillTest) {
            var response = await _skillTestService.GetSimilerSkillTestList(skillTest);
            return JsonExt(response);
        }
    }
}
