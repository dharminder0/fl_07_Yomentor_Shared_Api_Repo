using Core.Business.Entities.DataModels;
using Core.Common.Contracts;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Abstract {
    public interface IAddressRepository : IDataRepository<Business.Entities.DataModels.Address>{
        int UpsertAddress(Business.Entities.DataModels.Address address);
       Business.Entities.DataModels.Address GetUserAddress(int userId);
    }
}
