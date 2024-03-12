using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete
{
    public class BatchRepository : DataRepository<Batch>, IBatchRepository
    {
        public List<Batch> GetBatchDetailsbyId(int teacherId)
        {
            var sql =$"Select * from dbo.Batch where teacherid=@teacherId and isdeleted=0";
            var res=  Query<Batch>(sql, new {teacherId});
            return (List<Batch>)res;    
        }
    }
}
