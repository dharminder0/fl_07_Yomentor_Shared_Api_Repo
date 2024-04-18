﻿using Core.Business.Entities.DataModels;
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
                    skillTestResponse.AverageMarks = averageCount / userCount;
                    skillTestResponse.AteemptCount = userCount;

                } catch (Exception) {


                }

                skills.Add(skillTestResponse);
            }
            return skills;
        }
        public SkillTestResponse GetSkillTest(int id,int userId) {
            if (id > null) throw new ArgumentNullException(nameof(id));
            List<AteemptHistory> obj = new List<AteemptHistory>();
            var item = _skillTestRepository.GetSkillTest(id);

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
                skillTestResponse.AverageMarks = averageCount / userCount;
                skillTestResponse.AteemptCount = userCount;
            } catch (Exception) {


            }
            try {
              var response=  _skillTestRepository.GetAttemptHistory(userId, id);
                if (response != null) {
                    foreach (var item1 in response) {
                        AteemptHistory history = new AteemptHistory();
                        history.Score = item1.Score;
                        history.AttemptDate = item.CreateDate;
                        obj.Add(history);


                    }
                    skillTestResponse.AteemptHistory = obj;

                }
            } catch (Exception) {

            }

            return skillTestResponse;
        }
        public ActionMessageResponse UpsertAttempt(Attempt attempt) {
            var response = _skillTestRepository.UpsertAttempt(attempt);
            return new ActionMessageResponse { Content = response, Success = true };
        }
     
        public  async Task<List<AttemptSkillTestResponse>> GetQuizQuestionsWithAnswers(int skillTestId) {
            List<AttemptSkillTestResponse> quizResponseList = new List<AttemptSkillTestResponse>();

           var questionList= await  _skillTestRepository.GetQuestions(skillTestId);
            List<Question> questions = questionList.ToList();
            foreach (var question in questions) {
                 var answerList = await  _skillTestRepository.GetAnswerOptionsForQuestion(question.Id);
                List<AnswerOption> answerOptions= answerList.ToList();  

                quizResponseList.Add(new AttemptSkillTestResponse {
                    QuestionId = question.Id,
                    QuestionTitle = question.Title,
                    QuestionDescription = question.Description,
                    SkillTestId = question.SkillTestId,
                    QuestionCreateDate = question.CreateDate,
                    AnswerOptions = answerOptions
                });
            }

            return quizResponseList;
        }

    }
}

