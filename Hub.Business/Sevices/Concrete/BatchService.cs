using Core.Business.Entities.DataModels;
using Core.Business.Entities.DTOs;
using Core.Business.Sevices.Abstract;
using Core.Data.Repositories.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Core.Business.Entities.DTOs.Enum;

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
                    if (row.Days == null)
                    {
                       
                    }
                    else {
                        
                        List<Days> days = ConvertToDays(row.Days);
                        List<string> ob = new List<string>();
                        foreach (var day in days) {
                            ob.Add(day.ToString()); 
                        }

                        obj.Days=ob;
                        batchDtos.Add(obj);
                    }
                }

                return batchDtos;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                return null;
            }
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
