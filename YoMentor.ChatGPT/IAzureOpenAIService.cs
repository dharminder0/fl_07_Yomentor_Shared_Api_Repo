using Core.Business.Entities.ChatGPT;
using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Common.Web;
using Core.Data.Repositories.Abstract;
using Hub.Common.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace YoMentor.ChatGPT {
    public interface IAzureOpenAIService {
     
        
        Task<int> GenerateQuestions(QuestionRequest request);


    }

    public class AzureOpenAIService : ExternalServiceBase, IAzureOpenAIService {
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISkillTestRepository _skillTestRepository;  

        private readonly static  string _apiKey= "16599971ef99490cbae591a58b8ab0bd";
        private readonly static string _baseUrl= "https://jr-29-rgp-weu-ai.openai.azure.com/";
        private readonly string _modelName= "gpt-4o";

        public AzureOpenAIService(IGradeRepository gradeRepository, ISubjectRepository subjectRepository, ISkillTestRepository skillTestRepository) : base(_baseUrl, _apiKey) {
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository; 
            _skillTestRepository = skillTestRepository; 

            
           
        }


        private string CreateOpenAiApiUrl() {
            return $"{_baseUrl}openai/deployments/{_modelName}/chat/completions?api-version=2023-05-15";
        }





        public static string ExtractJsonPart(string input) {
            try {
     
                var parsedJson = JObject.Parse(input);
                return parsedJson.ToString();
            } catch (JsonReaderException ex) {
              
                Console.WriteLine("Failed to parse JSON directly. Error: " + ex.Message);
            }

            string jsonPattern = @"```json\s*(\{.*?\})\s*```";
            var jsonMatch = Regex.Match(input, jsonPattern, RegexOptions.Singleline);

            if (!jsonMatch.Success) {
                throw new Exception("JSON part not found");
            }

            string jsonContent = jsonMatch.Groups[1].Value;

            jsonContent = SanitizeJsonContent(jsonContent);

            try {
                // Attempt parsing the sanitized JSON content
                JObject jsonObject = JObject.Parse(jsonContent);

                // Patterns to extract title and summary
                string titlePattern = @"""title"":\s*""(.*?)"",";
                string summaryPattern = @"""summary"":\s*""(.*?)"",";

                var titleMatch = Regex.Match(jsonContent, titlePattern);
                var summaryMatch = Regex.Match(jsonContent, summaryPattern);

                // Check if title and summary are found
                if (!titleMatch.Success || !summaryMatch.Success) {
                    throw new Exception("Title or Summary not found");
                }

                // Extract title and summary values
                string title = titleMatch.Groups[1].Value.Trim();
                string summary = summaryMatch.Groups[1].Value.Trim();

                // Create the final combined JSON object
                var combinedJsonObject = new JObject {
                    ["title"] = title,
                    ["summary"] = summary,
                    ["questions"] = jsonObject["questions"]
                };

                return combinedJsonObject.ToString();
            } catch (JsonReaderException ex) {
                // Log the error and provide feedback on what went wrong
                Console.WriteLine("Failed to parse the JSON after extracting it. Error: " + ex.Message);
                throw new Exception("Failed to process JSON content: " + ex.Message);
            }
        }

        // Method to sanitize invalid escape sequences in JSON content
        private static string SanitizeJsonContent(string jsonContent) {
            // Regex to find invalid escape sequences (e.g., \i, \x, \q, etc.)
            string invalidEscapePattern = @"\\[^""\\/bfnrt]";


            jsonContent = Regex.Replace(jsonContent, invalidEscapePattern, "");

            return jsonContent;
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
        public static string ExtractJsonPartV2(string input) {
            try {

                var parsedJson = JObject.Parse(input);


                return parsedJson.ToString();
            } catch (JsonReaderException) {
            }

            string jsonPattern = @"```json\s*(\{.*?\})\s*```";
            var jsonMatch = Regex.Match(input, jsonPattern, RegexOptions.Singleline);

            // Check if the JSON part is found
            if (!jsonMatch.Success) {
                throw new Exception("JSON part not found");
            }

            string jsonContent = jsonMatch.Groups[1].Value;

            // Patterns to extract title and summary
            string titlePattern = @"""title"":\s*""(.*?)"",";
            string summaryPattern = @"""summary"":\s*""(.*?)"",";

            var titleMatch = Regex.Match(jsonContent, titlePattern);
            var summaryMatch = Regex.Match(jsonContent, summaryPattern);

            // Check if title and summary are found
            if (!titleMatch.Success || !summaryMatch.Success) {
                throw new Exception("Title or Summary not found");
            }

            // Extract title and summary values
            string title = titleMatch.Groups[1].Value.Trim();
            string summary = summaryMatch.Groups[1].Value.Trim();

            // Parse the entire JSON object
            JObject jsonObject = JObject.Parse(jsonContent);

            // Create the final combined JSON object
            var combinedJsonObject = new JObject {
                ["title"] = title,
                ["summary"] = summary,
                ["questions"] = jsonObject["questions"]
            };

            return combinedJsonObject.ToString();
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
                var apiUrl = $"openai/deployments/{_modelName}/chat/completions?api-version=2023-05-15";



                _httpService.AddHeader("api-key", $"{_apiKey}");

                var response = await _httpService.PostAsync<object>(apiUrl, openAiRequest);
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

        private (bool Success, object Result) ValidateRequest(QuestionRequest request) {

            string categoryName = _gradeRepository.GetCategorieName(request.Category);

            if (string.IsNullOrEmpty(categoryName)) {
                return (false, new { error = "Invalid category" });
            }


            return (true, categoryName);
        }



        public (bool Success, Prompt Result) BuildUserPrompt(QuestionRequest request) {
            string gradeName = _gradeRepository.GetGradeName(request.AcademicClass);
            string subjectname = _subjectRepository.GetSubjectName(request.Subject);
            string categoryName = _gradeRepository.GetCategorieName(request.Category);

            if (string.IsNullOrEmpty(categoryName)) {
                return (false, null);
            }

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
                isEnableTimer = request.isEnableTimer,
                TimerValue = request.TimerValue,


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
