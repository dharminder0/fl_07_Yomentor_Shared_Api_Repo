using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;

namespace Core.Business.Sevices.Concrete {
    public class ClassService :IclassService{
        private readonly IClassRepository _classRepository;

        public ClassService(IClassRepository classRepository) {
            _classRepository = classRepository;
        }
        public Users GetUsers() {
            return _classRepository.GetUsers();    
            
        }
    }
}
