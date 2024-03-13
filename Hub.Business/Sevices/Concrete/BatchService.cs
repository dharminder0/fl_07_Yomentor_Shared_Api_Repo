using Core.Business.Entities.DTOs;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using Newtonsoft.Json;
using static Core.Business.Entities.DTOs.Enum;

namespace Core.Business.Sevices.Concrete {
    public class BatchService : IBatchService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IClassRepository _classRepository;
        private readonly IGradeRepository _gradeRepository; 
        private readonly ISubjectRepository _subjectRepository;
        public BatchService(IBatchRepository batchRepository, IClassRepository classRepository, IGradeRepository gradeRepository, ISubjectRepository subjectRepository)
        {
            _batchRepository = batchRepository;
            _classRepository = classRepository;
            _gradeRepository = gradeRepository;
            _subjectRepository = subjectRepository;
        }
        public List<BatchDto> BatchDetailsByTeacherId(int teacherId)
        {
            BatchDto obj = new BatchDto(); 
            if (teacherId <= 0)
            {
                throw new Exception("Teacher Id is blank!!");
            }

            try
            {
                var res = _batchRepository.GetBatchDetailsbyId(teacherId);
                
                List<BatchDto> batchDtos = new List<BatchDto>();
                foreach (var row in res)
                {
                    string className = _gradeRepository.GetGradeName(row.GradeId);
                    string subjectName= _subjectRepository.GetSubjectName(row.SubjectId);
                    if (row.Days == null)
                    {
                        obj = new BatchDto();
                        obj.StartDate = row.StartDate;
                        obj.UpdateDate = row.UpdateDate;
                        obj.CreateDate = row.CreateDate;
                        obj.Description= row.Description;
                        obj.TuitionTime= row.TuitionTime;   
                        obj.ClassName = className;
                        obj.SubjectName = subjectName;
                        obj.Fee= row.Fee;
                        obj.StudentCount= row.StudentCount;
                        obj.Id= row.Id;
                        obj.Status= System.Enum.GetName(typeof(Status), row.Status); 
                        obj.FeeType= System.Enum.GetName(typeof(FeeType), row.FeeType);
                        batchDtos.Add(obj);
                    }
                    else {
                        
                        List<Days> days = ConvertToDays(row.Days);
                        List<string> ob = new List<string>();
                        foreach (var day in days) {
                            ob.Add(day.ToString()); 
                        }
                        obj = new BatchDto();
                        obj.StartDate = row.StartDate;
                        obj.UpdateDate = row.UpdateDate;
                        obj.CreateDate = row.CreateDate;
                        obj.Description = row.Description;
                        obj.TuitionTime = row.TuitionTime;
                        obj.ClassName = className;
                        obj.SubjectName = subjectName;
                        obj.Fee = row.Fee;
                        obj.StudentCount = row.StudentCount;
                        obj.Status = System.Enum.GetName(typeof(Status), row.Status);
                        obj.FeeType = System.Enum.GetName(typeof(FeeType), row.FeeType);
                        obj.Id = row.Id;
                        obj.Days=ob;
                        batchDtos.Add(obj);
                    }
                }

                return batchDtos;
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
            obj.FeeType = batchDetailRequest.FeeType;
            obj.Description = batchDetailRequest.Description;
            obj.Name = batchDetailRequest.Name;
            obj.NumberOfStudents = batchDetailRequest.NumberOfStudents;
            obj.ClassTime = batchDetailRequest.ClassTime;


            int id = await _batchRepository.InsertBatchDetails(obj);
            return new ActionMassegeResponse { Content = id, Message = "Btach Created", Response = true };
        }



        private List<Days> ConvertToDays(string response)
        {
          
            string[] numbers = response.Split(',');
            
            List<Days> days = new List<Days>();
            foreach (string num in numbers)
            {
                if (int.TryParse(num, out int dayNumber))
                {
                    // Convert the day number to Days enum
                    if (Days.IsDefined(typeof(Days), dayNumber))
                    {
                        days.Add((Days)dayNumber);
                    }
                }
            }
            return days;

        }
    }

}
