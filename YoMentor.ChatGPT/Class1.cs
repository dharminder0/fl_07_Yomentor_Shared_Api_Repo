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
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Core.Common.Extensions;
using static Core.Business.Entities.ChatGPT.ChatGPTRequest;
using System.Net.Http.Headers;

namespace YoMentor.ChatGPT {
    public interface IAIQuestionAnswerService {
        Task<object> GenerateQuestionsOld(QuestionRequest request);
        Task<object> GenerateQuestions(QuestionRequest request, bool isOnlyobject);
        
        Task<object> GenerateQuestions(QuestionRequest request);
    }

    public class AIQuestionAnswerService : ExternalServiceBase, IAIQuestionAnswerService {
        private static readonly Dictionary<string, List<string>> _userQuestions = new Dictionary<string, List<string>>();
        public AIQuestionAnswerService() : base("https://api.openai.com", GlobalSettings.ChatGPTKey) { }


        public async Task<object> GenerateQuestionsOld(QuestionRequest request) {
            var openAiRequest = new {
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

            var openAiRequest = new {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = $"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations." }
            },
                //$"Generate {request.NumberOfQuestions} questions for a {request.AcademicClass} class in {request.Subject} on the topic of {request.Topic} at a {request.ComplexityLevel} complexity level. Provide the correct answers and explanations.",
                max_tokens = 1500
            };


            var responseData = await _httpService.PostAsync<object>("v1/chat/completions", openAiRequest);

            var responseDatass = JObject.Parse(responseData.ToString());
            if (isOnlyobject) {
                return responseDatass;
            }

            var choices = responseDatass["choices"];
            var questions = new List<QuestionResponse>();
            foreach (var choice in choices) {
                var messageContent = choice["message"]["content"].ToString();
                var questionBlocks = messageContent.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var block in questionBlocks) {
                    var questionMatch = Regex.Match(block, @"Question: (.+)");
                    var answerMatch = Regex.Match(block, @"Answer: (.+)");
                    var explanationMatch = Regex.Match(block, @"Explanation: (.+)", RegexOptions.Singleline);

                    if (questionMatch.Success && answerMatch.Success && explanationMatch.Success) {
                        questions.Add(new QuestionResponse {
                            Question = questionMatch.Groups[1].Value.Trim(),
                            CorrectAnswer = answerMatch.Groups[1].Value.Trim(),
                            Explanation = explanationMatch.Groups[1].Value.Trim()
                        });
                    }
                }

            }
            return questions;
        }

        

      

        public async Task<object> GenerateQuestions(QuestionRequest request) {
            try {
                var validationResult = ValidateRequest(request);
                if (!validationResult.Success) {
                    return validationResult.Result;
                }

                var userPromptResult = BuildUserPrompt(request);
                if (!userPromptResult.Success) {
                    return userPromptResult.Result;
                }

                var openAiRequest = BuildOpenAiRequest(userPromptResult.Result);

                var response = await _httpService.PostAsync<object>("v1/chat/completions", openAiRequest);
                var processedResponse = ProcessOpenAiResponse(response);

                return processedResponse;
            } catch (Exception ex) {
                return new { error = $"An error occurred: {ex.Message}" };
            }
        }



        private (bool Success, object Result) ValidateRequest(QuestionRequest request) {
            if (request == null || string.IsNullOrEmpty(request.UserId)) {
                return (false, new { error = "User ID is required" });
            }

            if (request.Category == "Academic") {
                if (string.IsNullOrEmpty(request.AcademicClass) || string.IsNullOrEmpty(request.Subject) ||
                    string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.ComplexityLevel) ||
                    request.NumberOfQuestions <= 0) {
                    return (false, new { error = "Missing required fields for Academic category" });
                }
            }
            else if (request.Category == "Competitive Exams") {
                if (string.IsNullOrEmpty(request.ExamName) || string.IsNullOrEmpty(request.Subject) ||
                    string.IsNullOrEmpty(request.Topic) || string.IsNullOrEmpty(request.ComplexityLevel) ||
                    request.NumberOfQuestions <= 0) {
                    return (false, new { error = "Missing required fields for Competitive Exams category" });
                }
            }
            else {
                return (false, new { error = "Invalid category" });
            }

            return (true, null);
        }

        
        private (bool Success, string Result) BuildUserPrompt(QuestionRequest request) {
            string userPrompt = string.Empty;

            if (request.Category == "Academic") {
                userPrompt = $@"
Generate {request.NumberOfQuestions} questions for an Indian student in {request.AcademicClass} class studying {request.Subject} on the topic of {request.Topic} according to the NCERT syllabus and CBSE board standards. 
The questions should be of {request.ComplexityLevel} complexity and should test the student's critical thinking, problem-solving skills, and deep understanding of the topic.
Each question should include multiple choice answers, the correct answer, and a detailed explanation. 
Ensure that the generated questions do not repeat any of the previously provided questions.
The output should be in the following JSON array format:

[
    {{
        'question': 'Question 1',
        'choices': ['Option A', 'Option B', 'Option C', 'Option D'],
        'correct_answer': 'Option B',
        'explanation': 'Detailed explanation of why Option B is correct.'
    }},
    {{
        'question': 'Question 2',
        'choices': ['Option A', 'Option B', 'Option C', 'Option D'],
        'correct_answer': 'Option A',
        'explanation': 'Detailed explanation of why Option A is correct.'
    }},
    ...
]";
            }
            else if (request.Category == "Competitive Exams") {
                userPrompt = $@"
Generate {request.NumberOfQuestions} questions for an Indian student preparing for {request.ExamName} in the subject of {request.Subject} on the topic of {request.Topic}. 
The questions should be of {request.ComplexityLevel} complexity and should test the student's critical thinking, problem-solving skills, and deep understanding of the topic.
Ensure that the generated questions do not repeat any of the previously provided questions.
Each question should include multiple choice answers, the correct answer, and a detailed explanation. 
The output should be in the following JSON array format:

[
    {{
        'question': 'Question 1',
        'choices': ['Option A', 'Option B', 'Option C', 'Option D'],
        'correct_answer': 'Option B',
        'explanation': 'Detailed explanation of why Option B is correct.'
    }},
    {{
        'question': 'Question 2',
        'choices': ['Option A', 'Option B', 'Option C', 'Option D'],
        'correct_answer': 'Option A',
        'explanation': 'Detailed explanation of why Option A is correct.'
    }},
    ...
]";
            }

            return (true, userPrompt);
        }

        
        private object BuildOpenAiRequest(string userPrompt) {
            return new {
                model = "gpt-3.5-turbo",
                messages = new[] {
            new { role = "system", content = "You are an AI tutor assisting students in generating study questions based on their inputs." },
            new { role = "user", content = userPrompt }
        },
                max_tokens = 1500,
                n = 1,
                stop = "None",
                temperature = 0.7
            };
        }

      
        private (bool Success, object Result) ProcessOpenAiResponse(object responseData) {
            var responseJson = JObject.Parse(responseData.ToString());

            var choices = responseJson["choices"];
            var questions = new List<QuestionResponse>();
            foreach (var choice in choices) {
                var messageContent = choice["message"]["content"].ToString();
                var questionsArray = JArray.Parse(messageContent);

                foreach (var questionObj in questionsArray) {
                    var question = questionObj["question"].ToString();
                    var correctAnswer = questionObj["correct_answer"].ToString();
                    var explanation = questionObj["explanation"].ToString();
                    var options = questionObj["choices"].ToObject<List<string>>();

                    questions.Add(new QuestionResponse {
                        Question = question,
                        Choices = options,
                        CorrectAnswer = correctAnswer,
                        Explanation = explanation
                    });
                }
            }
            return (true, questions);
        }




    }
}
