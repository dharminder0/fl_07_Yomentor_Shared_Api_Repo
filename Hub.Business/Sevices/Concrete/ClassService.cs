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

        public Users GetTeacherById(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Id cannot be zero and null");
            }
            try
            {
                return _classRepository.GetTeacherById(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
