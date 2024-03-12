using Core.Business.Entities.DataModels;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete
{
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IClassRepository _classRepository;
        public BatchService(IBatchRepository batchRepository, IClassRepository classRepository)
        {
            _batchRepository = batchRepository;
            _classRepository = classRepository;
        }
        public List<Batch> BatchDetailsByTeacherId(int teacherId)
        {
            if (teacherId <= 0 )
            {
                throw new Exception("Teacher Id is blank!!");
            }
            try
            {
            var res=_batchRepository.GetBatchDetailsbyId(teacherId);  
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
