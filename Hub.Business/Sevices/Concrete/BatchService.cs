using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Newtonsoft.Json;
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
        public async Task<ActionMassegeResponse> AddBatchDetails(BatchDetailRequestV2 batchDetailRequest) {
            BatchDetailRequest obj = new BatchDetailRequest();
            obj.SubjectId = batchDetailRequest.SubjectId;   
            obj.TeacherId = batchDetailRequest.TeacherId;   
            obj.GradeId = batchDetailRequest.GradeId;
            obj.Days = JsonConvert.SerializeObject(batchDetailRequest.Days);
           
            
            obj.Date = DateTime.Now;
            obj.Fee = batchDetailRequest.Fee;   
            obj.FeeType= batchDetailRequest.FeeType;    
            obj.Description = batchDetailRequest.Description;       
            obj.Name = batchDetailRequest.Name; 
            obj.NumberOfStudents = batchDetailRequest.NumberOfStudents; 
            obj.ClassTime = batchDetailRequest.ClassTime;
            
            
        int id=    await  _batchRepository.InsertBatchDetails(obj);    
            return new ActionMassegeResponse { Content = id ,Message="Btach Created",Response=true};

        }
    }
}
