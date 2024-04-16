using Core.Business.Entities.RequestModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class SkillTestService : ISkillTestService{
        private readonly ISkillTestRepository _skillTestRepository;
        private readonly IGradeRepository _gradeRepository; 
        private readonly ISubjectRepository _subjectRepository; 
        public SkillTestService(ISkillTestRepository skillTestRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository)
        {
            _skillTestRepository = skillTestRepository;    
            _gradeRepository = gradeRepository; 
            _subjectRepository = subjectRepository; 
        }

        public  async Task<List<SkillTestResponse>> GetSkillTestList(SkillTestRequest skillTest) {
            if(skillTest == null) throw new ArgumentNullException(nameof(skillTest));
            List<SkillTestResponse> skills = new List<SkillTestResponse>();
            var response= await _skillTestRepository.GetSkillTestList(skillTest);
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
                string subjectName=_subjectRepository.GetSubjectName(skillTestResponse.SubjectId);  
                if(!string.IsNullOrWhiteSpace(subjectName)) { 
                    skillTestResponse.SubjectName = subjectName;    
                }
                try {
                    var averageCount = _skillTestRepository.GetSkillTestSumScore(skillTestResponse.Id);
                    int userCount = _skillTestRepository.GetSkillTestUser(skillTestResponse.Id);
                    skillTestResponse.AverageMarks = averageCount / userCount;

                } catch (Exception) {

                   
                }
             
                skills.Add(skillTestResponse);  
            }
            return skills;
        }
        public SkillTestResponse GetSkillTest(int id) {
            if (id > null) throw new ArgumentNullException(nameof(id));    
            var item =  _skillTestRepository.GetSkillTest(id);
            
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
            } catch (Exception) {

             
            }
  

               return skillTestResponse;    
            }
            
        }
    }

