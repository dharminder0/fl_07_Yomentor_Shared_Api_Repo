using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class SubjectService:ISubjectService {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService(ISubjectRepository subjectRepository)
        {
                _subjectRepository = subjectRepository; 
        }
        public async Task<List<SubjectResponse>> GetAllSubjects(int gradeId) {
            List<SubjectResponse> response = new List<SubjectResponse>();  
           var sujects= await  _subjectRepository.GetAllSubjects(gradeId);
            foreach (var item in sujects) {
                SubjectResponse subjectResponse = new SubjectResponse();    
                subjectResponse.Id = item.Id;
                subjectResponse.GradeId = item.GradeId; 
                subjectResponse.Name = item.Name;   
                subjectResponse.IsDeleted = item.IsDeleted;


                response.Add(subjectResponse);  

            }
            return response;    
        }
    }
}
