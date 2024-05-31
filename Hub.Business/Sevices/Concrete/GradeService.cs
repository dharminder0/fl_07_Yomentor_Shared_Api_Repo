using Core.Business.Entities.DataModels;
using Core.Business.Entities.ResponseModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class GradeService:IGradeService {
        private readonly IGradeRepository _gradeRepository;
        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository=gradeRepository;
        }

        public async Task<List<GradeResponse>> GetAllGrades(int type) {
            var grades= await _gradeRepository.GetAllGrades(type);
            List<GradeResponse> results= new List<GradeResponse>(); 
            foreach (var item in grades) {
                GradeResponse response = new GradeResponse();
                response.Id = item.Id;  
                response.Name = item.Name;  
                response.Isdeleted=item.Isdeleted;  
                response.Type = item.Type;
                results.Add(response);
            }
          return results;

      

        }
    }
}
