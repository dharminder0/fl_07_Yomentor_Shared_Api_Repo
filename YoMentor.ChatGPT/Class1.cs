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
using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Data.Repositories.Abstract;
using Core.Data.Repositories.Concrete;
using static Core.Business.Entities.DTOs.Enum;

namespace YoMentor.ChatGPT {
    public interface IAIQuestionAnswerService {
        Task<object> GenerateQuestionsOld(QuestionRequest request);
        Task<object> GenerateQuestions(QuestionRequest request, bool isOnlyobject);

        Task<int> GenerateQuestions(QuestionRequest request);
    }

    public class AIQuestionAnswerService : ExternalServiceBase, IAIQuestionAnswerService {
        private static readonly Dictionary<string, List<string>> _userQuestions = new Dictionary<string, List<string>>();
        private readonly ISkillTestRepository _skillTestRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        public AIQuestionAnswerService(ISkillTestRepository skillTestRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository) : base("https://api.openai.com", GlobalSettings.ChatGPTKey) {
            _skillTestRepository = skillTestRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
        }


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
                max_tokens = 3000
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
                            correct_answer = answerMatch.Groups[1].Value.Trim(),
                            Explanation = explanationMatch.Groups[1].Value.Trim()
                        });
                    }
                }

            }
            return questions;
        }





        public async Task<int> GenerateQuestions(QuestionRequest request) {
            try {
                int skillTestId = 0;
                var validationResult = ValidateRequest(request);
                if (!validationResult.Success) {
                    return 0;
                }

                var userPromptResult = BuildUserPrompt(request);

                if (!userPromptResult.Success) {
                    return 0;
                }

                var openAiRequest = BuildOpenAiRequest(userPromptResult.Result);

                var response = await _httpService.PostAsync<object>("v1/chat/completions", openAiRequest);
                var processedResponse = ProcessOpenAiResponse(response);
                try {
                    List<QuestionInfo> questionInfos = new List<QuestionInfo>();
                    foreach (var item in processedResponse.Questions) {

                        Questions questions = new Questions();
                        questions.CorrectAnswer = item.correct_answer;
                        questions.Explanation = item.Explanation;
                        questions.QuestionText = item.Question;
                        questions.Choices = (item.Choices);
                        QuestionInfo questionInfo = new QuestionInfo();
                        questionInfo.Title = questions.QuestionText;
                        questionInfo.Description = questions.Explanation;
                        questionInfo.SkillTestId = 0;
                        questionInfo.CorrectOption = questions.CorrectAnswer;

                        List<AnswerInfo> answerInfos = new List<AnswerInfo>();

                        for (int i = 0; i < item.Choices.Count; i++) {
                            AnswerInfo answerInfo = new AnswerInfo();
                            answerInfo.Title = item.Choices[i];
                            answerInfo.IsCorrect = (questions.Choices[i] == questions.CorrectAnswer);
                            answerInfos.Add(answerInfo);
                        }

                        questionInfo.AnswerOptions = answerInfos;
                        questionInfos.Add(questionInfo);
                    }

                    ProcessedResponse processedResponse1 = new ProcessedResponse();
                    processedResponse1.Questions = questionInfos;
                    processedResponse1.Summary = processedResponse.Summary;
                    processedResponse1.Title = processedResponse.Title;
                    skillTestId = await InsertSkillTestWithQuestionsAndAnswerOptions(processedResponse1, request);
                    processedResponse.SkillTestId = skillTestId;


                } catch (Exception ex) {

                }


                return skillTestId;
            } catch (Exception ex) {
                return 0;
            }
        }




        private (bool Success, object Result) ValidateRequest(QuestionRequest request) {

            if (request.Category == (int)Category.Academic) {
                return (true, Category.Academic.ToString());
            }

            else if (request.Category == (int)Category.Competitive_Exams) {
                return (true, Category.Competitive_Exams.ToString());
            }
            else {
                return (false, new { error = "Invalid category" });
            }
        }




        private (bool Success, string Result) BuildUserPrompt(QuestionRequest request) {
            string userPrompt = string.Empty;
            string gradeName = _gradeRepository.GetGradeName(request.AcademicClass);
            string subjectname = _subjectRepository.GetSubjectName(request.Subject);
            string categoryName = Enum.GetName(typeof(Category), request.Category);
            var promptData = _skillTestRepository.GetPrompt(categoryName);
            string complexityLevel = Enum.GetName(typeof(ComplexityLevel), request.ComplexityLevel);
            string language = Enum.GetName(typeof(Language), request.Language);

            if (promptData != null) {
                userPrompt = promptData.Prompt_Text;


                userPrompt = userPrompt.Replace("{request.NumberOfQuestions}", request.NumberOfQuestions.ToString());
                userPrompt = userPrompt.Replace("{request.AcademicClass}", gradeName);
                userPrompt = userPrompt.Replace("{request.Subject}", subjectname);
                userPrompt = userPrompt.Replace("{request.Topic}", request.Topic);
                userPrompt = userPrompt.Replace("{request.ComplexityLevel}", complexityLevel);
                //userPrompt = userPrompt.Replace("{request.ExamName}", gradeName);
                userPrompt = userPrompt.Replace("{request.language}", language);


            }
            else {
                return (false, "Prompt not found for the specified category.");
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
                max_tokens = 3500,
                n = 1,
                stop = "None",
                temperature = 0.7
            };
        }
        private Questionnaire ProcessOpenAiResponse(object responseData) {
            try {
                var responseJson = JObject.Parse(responseData.ToString());
                var messageContent = responseJson["choices"][0]["message"]["content"].ToString();

                var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(messageContent);
                return questionnaire;
            } catch (Exception ex) {
                // Log the exception for debugging purposes
                Console.WriteLine($"Error processing JSON response: {ex.Message}");
                return null;
            }
        }


        private bool IsValidJson(string json) {
            try {
                JToken.Parse(json);
                return true;
            } catch (JsonReaderException) {
                return false;
            }
        }





        private async Task<int> InsertSkillTestWithQuestionsAndAnswerOptions(ProcessedResponse processedResponse, QuestionRequest request) {



            var skillTest = new SkillTest {
                Title = processedResponse.Title,
                GradeId = request.AcademicClass,
                SubjectId = request.Subject,
                UpdateDate = DateTime.Now,
                CreateDate = DateTime.UtcNow,
                IsDeleted = false,
                Description = processedResponse.Summary,
            };

            int skillTestId = await _skillTestRepository.InsertSkillTest(skillTest);

            foreach (var question in processedResponse.Questions) {

                int questionId = await _skillTestRepository.InsertQuestion(new Question {
                    Title = question.Title,
                    Explanations = question.Description,
                    SkillTestId = skillTestId,
                    //CorrectOption =  int.Parse(question.CorrectOption),
                    CreateDate = DateTime.UtcNow
                });

                foreach (var answerOption in question.AnswerOptions) {

                    await _skillTestRepository.InsertAnswerOption(new AnswerOption {
                        QuestionId = questionId,
                        Title = answerOption.Title,
                        IsCorrect = answerOption.IsCorrect,
                        CreateDate = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
            }
            return skillTestId;
        }

    }


}

