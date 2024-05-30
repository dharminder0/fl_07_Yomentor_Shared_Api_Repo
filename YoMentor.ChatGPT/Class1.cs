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
using Core.Business.Entities.DataModels;

namespace YoMentor.ChatGPT
{
    public interface IAIQuestionAnswerService
    {
        Task<object> GenerateQuestionsOld(QuestionRequest request);
        Task<object> GenerateQuestions(QuestionRequest request, bool isOnlyobject);
    }

    public class AIQuestionAnswerService : ExternalServiceBase, IAIQuestionAnswerService
    {

        public AIQuestionAnswerService() : base("https://api.openai.com", GlobalSettings.ChatGPTKey) { }


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


        public async Task<object> GenerateQuestions(QuestionRequest request, bool isOnlyobject) {

            //var openAiRequest = new
            //{
            //    model = "gpt-3.5-turbo",
            //    messages = new[]
            //    {
            //    new { role = "system", content = "You are a helpful assistant." },
            //    new { role = "user", content = $"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations." }
            //},
            //    //$"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations.",
            //    max_tokens = 1500
            //};

            string userPrompt = $@"
    Generate {request.NumberOfQuestions} questions for an Indian student in {request.AcademicClass} class studying {request.Subject} on the topic of {request.Topic} according to the NCERT syllabus and CBSE board standards. 
    The questions should be of {request.ComplexityLevel} complexity and should test the student's critical thinking, problem-solving skills, and deep understanding of the topic.
    Each question should include multiple choice answers, the correct answer, and a detailed explanation. 
    Ensure that the generated questions do not repeat any of the previously provided questions.
    The output should be in the following JSON array format:

    [
        {{
            ""question"": ""Question 1"",
            ""choices"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
            ""correct_answer"": ""Option B"",
            ""explanation"": ""Detailed explanation of why Option B is correct.""
        }},
        {{
            ""question"": ""Question 2"",
            ""choices"": [""Option A"", ""Option B"", ""Option C"", ""Option D""],
            ""correct_answer"": ""Option A"",
            ""explanation"": ""Detailed explanation of why Option A is correct.""
        }},
        ...
    ]
";

            var openAiRequest = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "You are an AI tutor assisting students in generating study questions based on their inputs." },
                new { role = "user", content =userPrompt }
            },
                //$"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations.",
                max_tokens = 1500,
                n = 1,
                stop = "None",
                temperature = 0.7
            };






            var responseData = await _httpService.PostAsync<object>("v1/chat/completions", openAiRequest);

            var responseDatass = JObject.Parse(responseData.ToString());
            if (isOnlyobject) {
                return responseDatass;
            }
            
            var choices = responseDatass["choices"];
            var questions = new List<QuestionResponse>();
            foreach (var choice in choices)
            {
                var messageContent = choice["message"]["content"].ToString();
                var questionBlocks = messageContent.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var block in questionBlocks)
                {
                    var questionMatch = Regex.Match(block, @"question: (.+)");
                    var answerMatch = Regex.Match(block, @"answer: (.+)");
                    var explanationMatch = Regex.Match(block, @"explanation: (.+)");

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
