using Core.Business.Entities.ChatGPT;
using Core.Common.Web;
using Hub.Common.Settings;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace YoMentor.ChatGPT
{
    public interface IAIQuestionAnswerService
    {
        Task<object> GenerateQuestionsOld(QuestionRequest request);
        Task<object> GenerateQuestions(QuestionRequest request);
    }

    public class AIQuestionAnswerService : ExternalServiceBase, IAIQuestionAnswerService
    {

        public AIQuestionAnswerService() : base("https://api.openai.com", "Bearer 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") { }


        public async Task<object> GenerateQuestionsOld(QuestionRequest request)
        {
            var openAiRequest = new
            {
                prompt = $"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations.",
                max_tokens = 150
            };

            var response = await _httpService.PostAsync<List<QuestionResponse>>("v1/chat/completions", openAiRequest);

            return response;
            // Process the response and map it to the QuestionResponse model
            // For simplicity, let's assume the response is directly usable (you'll likely need to parse and format it properly)
            //var questions = JsonSerializer.Deserialize<List<QuestionResponse>>(response.);

            //return questions;
        }


        public async Task<object> GenerateQuestions(QuestionRequest request) {
           
            var openAiRequest = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = $"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations." }
            },
                max_tokens = 1500
            };

           
            var responseData = await _httpService.PostAsync<object>("v1/chat/completions", openAiRequest);

            var responseDatass = JObject.Parse(responseData.ToString());
            var choices = responseDatass["choices"];
            var questions = new List<QuestionResponse>();
            foreach (var choice in choices)
            {
                var messageContent = choice["message"]["content"].ToString();
                var questionBlocks = messageContent.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var block in questionBlocks)
                {
                    var questionMatch = Regex.Match(block, @"Question: (.+)");
                    var answerMatch = Regex.Match(block, @"Answer: (.+)");
                    var explanationMatch = Regex.Match(block, @"Explanation: (.+)");

                    if (questionMatch.Success && answerMatch.Success && explanationMatch.Success)
                    {
                        questions.Add(new QuestionResponse
                        {
                            Question = questionMatch.Groups[1].Value.Trim(),
                            CorrectAnswer = answerMatch.Groups[1].Value.Trim(),
                            Explanation = explanationMatch.Groups[1].Value.Trim()
                        });
                    }
                }
            }
            return questions;
        }
    }
}
