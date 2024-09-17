using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Sevices.Concrete {
    public class SkillTestService : ISkillTestService {
        private readonly ISkillTestRepository _skillTestRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ISubjectRepository _subjectRepository;
        public SkillTestService(ISkillTestRepository skillTestRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository) {
            _skillTestRepository = skillTestRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
        }

        public async Task<List<SkillTestResponse>> GetSkillTestList(SkillTestRequest skillTest) {
            if (skillTest == null) throw new ArgumentNullException(nameof(skillTest));
            List<SkillTestResponse> skills = new List<SkillTestResponse>();
            var response = await _skillTestRepository.GetSkillTestList(skillTest);
            foreach (var item in response) {
                SkillTestResponse skillTestResponse = new SkillTestResponse();
                skillTestResponse.Id = item.Id;
                skillTestResponse.SubjectId = item.SubjectId;
                skillTestResponse.Title = item.Title;
                skillTestResponse.Description = item.Description;
                skillTestResponse.UpdateDate = item.UpdateDate;
                skillTestResponse.IsDeleted = item.IsDeleted;
                skillTestResponse.GradeId = item.GradeId;
                string gradeName = _gradeRepository.GetGradeName(skillTestResponse.GradeId);
                if (!string.IsNullOrWhiteSpace(gradeName)) {
                    skillTestResponse.GradeName = gradeName;
                }
                string subjectName = _subjectRepository.GetSubjectName(skillTestResponse.SubjectId);
                if (!string.IsNullOrWhiteSpace(subjectName)) {
                    skillTestResponse.SubjectName = subjectName;
                }
                try {
                    var averageCount = _skillTestRepository.GetSkillTestSumScore(skillTestResponse.Id);
                    int userCount = _skillTestRepository.GetSkillTestUser(skillTestResponse.Id);
                    skillTestResponse.AverageMarks = averageCount;
                    skillTestResponse.AttemptCount = userCount;

                } catch (Exception) {


                }

                skills.Add(skillTestResponse);
            }
            return skills;
        }
        public async Task<List<SkillTestResponse>> GetSkillTestListByUser(SkillTestRequest skillTest) {
            if (skillTest == null) throw new ArgumentNullException(nameof(skillTest));
            List<SkillTestResponse> skills = new List<SkillTestResponse>();
            var response = await _skillTestRepository.GetSkillTestListByUser(skillTest);
            foreach (var item in response) {
                SkillTestResponse skillTestResponse = new SkillTestResponse();
                skillTestResponse.Id = item.Id;
                skillTestResponse.SubjectId = item.SubjectId;
                skillTestResponse.Title = item.Title;
                skillTestResponse.Description = item.Description;
                skillTestResponse.UpdateDate = item.UpdateDate;
                skillTestResponse.IsDeleted = item.IsDeleted;
                skillTestResponse.GradeId = item.GradeId;
                string gradeName = _gradeRepository.GetGradeName(skillTestResponse.GradeId);
                if (!string.IsNullOrWhiteSpace(gradeName)) {
                    skillTestResponse.GradeName = gradeName;
                }
                string subjectName = _subjectRepository.GetSubjectName(skillTestResponse.SubjectId);
                if (!string.IsNullOrWhiteSpace(subjectName)) {
                    skillTestResponse.SubjectName = subjectName;
                }
                try {
                    var averageCount = _skillTestRepository.GetSkillTestSumScore(skillTestResponse.Id);
                    int userCount = _skillTestRepository.GetSkillTestUser(skillTestResponse.Id);
                    skillTestResponse.AverageMarks = averageCount;
                    skillTestResponse.AttemptCount = userCount;

                } catch (Exception) {


                }

                skills.Add(skillTestResponse);
            }
            return skills;
        }
        public SkillTestResponse GetSkillTest(int id,int userId) {
            if (id > null) throw new ArgumentNullException(nameof(id));
            List<AttemptHistory> obj = new List<AttemptHistory>();
            var item = _skillTestRepository.GetSkillTest(id);

            SkillTestResponse skillTestResponse = new SkillTestResponse();
            skillTestResponse.Id = item.Id;
            skillTestResponse.SubjectId = item.SubjectId;
            skillTestResponse.Title = item.Title;
            skillTestResponse.Description = item.Description;
            skillTestResponse.UpdateDate = item.UpdateDate;
            skillTestResponse.IsDeleted = item.IsDeleted;
            skillTestResponse.GradeId = item.GradeId;
            skillTestResponse.Complexity = item.Complexity_Level;
            skillTestResponse.NumberOfQuestions=item.NumberOf_Questions;
            skillTestResponse.Category = item.Prompt_Type;
            skillTestResponse.Language = item.LanguageId;
            string gradeName = _gradeRepository.GetGradeName(skillTestResponse.GradeId);
            if (!string.IsNullOrWhiteSpace(gradeName)) {
                skillTestResponse.GradeName = gradeName;
            }
            string subjectName = _subjectRepository.GetSubjectName(skillTestResponse.SubjectId);
            if (!string.IsNullOrWhiteSpace(subjectName)) {
                skillTestResponse.SubjectName = subjectName;
            }
            try {
                var averageCount = _skillTestRepository.GetSkillTestSumScore(skillTestResponse.Id);
                int userCount = _skillTestRepository.GetSkillTestUser(skillTestResponse.Id);
                skillTestResponse.AverageMarks = averageCount;
                skillTestResponse.AttemptCount = userCount;
            } catch (Exception) {


            }
            try {
              var response=  _skillTestRepository.GetAttemptHistory(userId, id);
                if (response != null) {
                    foreach (var item1 in response) {
                        AttemptHistory history = new AttemptHistory();
                        history.Score = item1.Score;
                        history.AttemptDate = item1.StartDate;
                        history.AttemptId= item1.Id;    
                        obj.Add(history);


                    }
                    skillTestResponse.AttemptHistory = obj;

                }
            } catch (Exception) {

            }

            return skillTestResponse;
        }
        public ActionMessageResponse UpsertAttempt(Attempt attempt) {
            var response = _skillTestRepository.UpsertAttempt(attempt);
            return new ActionMessageResponse { Content = response, Success = true };
        }

        public async Task<List<AttemptSkillTestResponse>> GetQuizQuestionsWithAnswers(int skillTestId, int attemptId) {
            List<AttemptSkillTestResponse> quizResponseList = new List<AttemptSkillTestResponse>();
            int selectedAnswerId = 0;

            var questionList = await _skillTestRepository.GetQuestions(skillTestId);
            List<Question> questions = questionList.ToList();
            foreach (var question in questions) {
                var answerList = await _skillTestRepository.GetAnswerOptionsForQuestion(question.Id);
                if (attemptId > 0) {
                    selectedAnswerId = _skillTestRepository.GetAnswerId(attemptId, question.Id);
                }
                List<AnswerOption> answerOptions = answerList.ToList();

                AttemptSkillTestResponse pb = new AttemptSkillTestResponse();
                pb.QuestionId = question.Id;
                pb.QuestionTitle = question.Title;
                pb.QuestionDescription = question.Description;
                pb.SkillTestId = question.SkillTestId;
                pb.QuestionCreateDate = question.CreateDate;
                pb.AnswerOptions = answerOptions;
                if(selectedAnswerId >0) {
                    pb.SelectedAnswerId = selectedAnswerId; 
                }
                quizResponseList.Add(pb);
            }

                return quizResponseList;
            }
        
        public ActionMessageResponse AttemptDetailBulkInsert(SkillTestAttemptRequest request) {
            if (request == null) throw new ArgumentNullException();
            var percentage = new AttemptSummaryResponse();
            _skillTestRepository.DeleteAttemptDetail(request.AttemptId);
            foreach (var item in request.AttemptedQuestions) {
                AttemptDetail attemptDetail=new AttemptDetail();
                attemptDetail.AttemptId=request.AttemptId;  
                attemptDetail.CreateDate=attemptDetail.CreateDate;  
                attemptDetail.QuestionId=item.QuestionId;   
                attemptDetail.AnswerId=item.AnswerId;
                try {
                   int id= _skillTestRepository.GetCorrectAnswer(item.QuestionId);
                    if (id == item.AnswerId) {
                        attemptDetail.IsCorrect=true;
                    }
                    
                } catch (Exception) {

                   
                }
               
                _skillTestRepository.InsertAttemptDetail(attemptDetail);
                 


              

            }
            percentage = _skillTestRepository.CalculatePercentage(request.AttemptId);
            if(percentage != null) {
                _skillTestRepository.UpdateScore(request.AttemptId,percentage.PercentageCorrect);
            }
            return new ActionMessageResponse { Success = true,Content= percentage,Message="Insertion_Successfully" };

        }
        public async Task<List<SkillTestResponse>> GetSimilerSkillTestList(SkillTestRequest skillTest) {
            if (skillTest == null) throw new ArgumentNullException(nameof(skillTest));
            List<SkillTestResponse> skills = new List<SkillTestResponse>();
            var response = await _skillTestRepository.GetSimilerSkillTestList(skillTest);
            foreach (var item in response) {
                SkillTestResponse skillTestResponse = new SkillTestResponse();
                skillTestResponse.Id = item.Id;
                skillTestResponse.SubjectId = item.SubjectId;
                skillTestResponse.Title = item.Title;
                skillTestResponse.Description = item.Description;
                skillTestResponse.UpdateDate = item.UpdateDate;
                skillTestResponse.IsDeleted = item.IsDeleted;
                skillTestResponse.GradeId = item.GradeId;
                string gradeName = _gradeRepository.GetGradeName(skillTestResponse.GradeId);
                if (!string.IsNullOrWhiteSpace(gradeName)) {
                    skillTestResponse.GradeName = gradeName;
                }
                string subjectName = _subjectRepository.GetSubjectName(skillTestResponse.SubjectId);
                if (!string.IsNullOrWhiteSpace(subjectName)) {
                    skillTestResponse.SubjectName = subjectName;
                }
                try {
                    var averageCount = _skillTestRepository.GetSkillTestSumScore(skillTestResponse.Id);
                    int userCount = _skillTestRepository.GetSkillTestUser(skillTestResponse.Id);
                    skillTestResponse.AverageMarks = averageCount;
                    skillTestResponse.AttemptCount = userCount;

                } catch (Exception) {


                }

                skills.Add(skillTestResponse);
            }
            return skills;
        }
    }
}

