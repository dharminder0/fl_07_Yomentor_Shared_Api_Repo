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
using System.Collections.Generic;

namespace YoMentor.ChatGPT {
    public interface IAIQuestionAnswerService {
        Task<object> GenerateQuestionsOld(QuestionRequest request);
        Task<object> GenerateQuestions(QuestionRequest request, bool isOnlyobject);

        Task<int> GenerateQuestions(QuestionRequest request);
     
        List<DailyAttemptCountV2> GetAttemptCountV2(int userId, SkillTestAttemptRange skillTest);
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
                 var processedResponse = await ProcessOpenAiResponse(response);
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

        public static string RemoveJsonDelimiters(string input) {
            // Remove ```json and ``` from the input string
            string pattern = @"^```json\s*|\s*```$";
            string result = Regex.Replace(input, pattern, string.Empty, RegexOptions.Multiline).Trim();

            return result;
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

        public (bool Success, Prompt Result) BuildUserPrompt(QuestionRequest request) {
            string gradeName = _gradeRepository.GetGradeName(request.AcademicClass);
            string subjectname = _subjectRepository.GetSubjectName(request.Subject);
            string categoryName = Enum.GetName(typeof(Category), request.Category);
            var promptData = _skillTestRepository.GetPrompt(categoryName);
            string complexityLevel = Enum.GetName(typeof(ComplexityLevel), request.ComplexityLevel);
            string language = Enum.GetName(typeof(Language), request.Language);

            if (promptData != null) {
                string userPrompt = promptData.Prompt_Text;
                userPrompt = userPrompt.Replace("{request.NumberOfQuestions}", request.NumberOfQuestions.ToString());
                userPrompt = userPrompt.Replace("{request.AcademicClass}", gradeName);
                userPrompt = userPrompt.Replace("{request.Subject}", subjectname);
                userPrompt = userPrompt.Replace("{request.Topic}", request.Topic);
                userPrompt = userPrompt.Replace("{request.ComplexityLevel}", complexityLevel);
                userPrompt = userPrompt.Replace("{request.language}", language);

                Prompt resultPrompt = new Prompt {
                    Prompt_Id = promptData.Prompt_Id,
                    Prompt_Text = userPrompt,
                    Prompt_Type = promptData.Prompt_Type,
                    Temperature = promptData.Temperature,
                    Max_tokens = promptData.Max_tokens,
                    Top_p = promptData.Top_p,
                    Sop_Sequence = promptData.Sop_Sequence,
                    Model = promptData.Model,
                    System_Role = promptData.System_Role,
                };

                return (true, resultPrompt);
            }
            else {
                return (false, null);
            }
        }




        private object BuildOpenAiRequest(Prompt userPrompt) {
            return new {
                model = userPrompt.Model,
                messages = new[] {
            new { role = "system", content = userPrompt.System_Role },
            new { role = "user", content = userPrompt.Prompt_Text }
        },
                max_tokens = userPrompt.Max_tokens,
                n = 1,
                stop = userPrompt.Sop_Sequence,
                temperature = userPrompt.Temperature,
                top_p = userPrompt.Top_p
            };
        }

        private async Task<Questionnaire> ProcessOpenAiResponse(object responseData) {
            try {
                var responseJson = JObject.Parse(responseData.ToString());
                var messageContent = responseJson["choices"][0]["message"]["content"].ToString();
                var res = ExtractJsonPart(messageContent);
                var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(res);
                return questionnaire;
            } catch (Exception ex) {
                Console.WriteLine($"Error processing JSON response: {ex.Message}");
                return null;
            }
        }

        public static string ExtractJsonPart(string input) {
            string jsonPattern = @"```json\s*(\{.*?\})\s*```"; 
            var jsonMatch = Regex.Match(input, jsonPattern, RegexOptions.Singleline);

            if (!jsonMatch.Success) {
                throw new Exception("JSON part not found");
            }

            string jsonObject = jsonMatch.Groups[1].Value;

            JObject parsedJson = JObject.Parse(jsonObject);

            string title = parsedJson["title"]?.ToString().Trim();
            string summary = parsedJson["summary"]?.ToString().Trim();

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(summary)) {
                throw new Exception("Title or Summary not found in the JSON object");
            }

            // Create a new combined JSON object with title, summary, and questions
            var combinedJsonObject = new JObject {
                ["title"] = title,
                ["summary"] = summary,
                ["questions"] = parsedJson["questions"] 
            };

            return combinedJsonObject.ToString(Newtonsoft.Json.Formatting.Indented);
        }

        //public static string ExtractJsonPart(string input) {

        //    string jsonPattern = @"```json\s*(\[.*?\])\s*```";
        //    var jsonMatch = Regex.Match(input, jsonPattern, RegexOptions.Singleline);

        //    if (!jsonMatch.Success) {
        //        throw new Exception("JSON part not found");
        //    }

        //    string jsonArray = jsonMatch.Groups[1].Value;

        //    string titlePattern = @"Title:\s*(.*?)\n";
        //    string summaryPattern = @"Summary:\s*(.*?)\n\n";

        //    var titleMatch = Regex.Match(input, titlePattern, RegexOptions.Singleline);
        //    var summaryMatch = Regex.Match(input, summaryPattern, RegexOptions.Singleline);

        //    if (!titleMatch.Success || !summaryMatch.Success) {
        //        throw new Exception("Title or Summary not found");
        //    }

        //    string title = titleMatch.Groups[1].Value.Trim();
        //    string summary = summaryMatch.Groups[1].Value.Trim();
        //    var combinedJsonObject = new JObject {
        //        ["title"] = title,
        //        ["summary"] = summary,
        //        ["questions"] = JArray.Parse(jsonArray)
        //    };

        //    return combinedJsonObject.ToString();
        //}
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
                Topic = request.Topic,
                Complexity_Level = request.ComplexityLevel,
                NumberOf_Questions = request.NumberOfQuestions,
                Prompt_Type = request.Category,
                CreatedBy = request.UserId,
                Language = request.Language,
                isEnableTimer=request.isEnableTimer,
                TimerValue=request.TimerValue,  

                
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

        public List<DailyAttemptCountV2> GetAttemptCountV2(int userId, SkillTestAttemptRange skillTest) {

            var response = _skillTestRepository.GetAttemptCounts(userId, skillTest);


            List<DailyAttemptCountV2> resultList = new List<DailyAttemptCountV2>();

 
            foreach (var item in response) {
                DailyAttemptCountV2 obj = new DailyAttemptCountV2 {
                    Label = item.GroupedDate.ToString("dd MMM yyyy"),
                    Value = item.AttemptedCount  
                };


                resultList.Add(obj);
            }


            return resultList;
        }

    }





}

