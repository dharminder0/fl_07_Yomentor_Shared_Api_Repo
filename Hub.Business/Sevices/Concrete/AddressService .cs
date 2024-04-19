using Core.Business.Entities.DataModels;
using Core.Business.Entities.RequestModels;
using Core.Business.Sevices.Abstract;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Sevices.Concrete {
    public class AddressService : IAddressService{
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository; 
                
        }
        public ActionMassegeResponse UpsertAddress(AddressRequest request) {
            if (request == null) throw new ArgumentNullException();
            Address address=new Address();
            address.Address1 = request.Address1;    
            address.Address2 = request.Address2;    
            address.UserId = request.UserId;    
            address.StateId = request.StateId;  
            address.City = request.City;    
            address.Latitude = request.Latitude;    
            address.Longitude = request.Longitude;  
            address.UpdateDate = request.UpdateDate;    
            address.CreateDate = request.CreateDate;    
            address.IsDeleted = request.IsDeleted;  
            address.Id = request.Id;
            address.Pincode = request.Pincode;
          int content=  _addressRepository.UpsertAddress(address);  
            return new ActionMassegeResponse { Content = content ,Message="Upsert Successfully",Response=true};

        }
        public Address GetUserAddress(int userId) {
            var addressInfo = _addressRepository.GetUserAddress(userId);
            if (addressInfo != null) {
                var address = new Address {
                    Address1 = addressInfo.Address1,
                    Address2 = addressInfo.Address2,
                    UserId = addressInfo.UserId,
                    StateId = addressInfo.StateId,
                    Latitude = addressInfo.Latitude,
                    Longitude = addressInfo.Longitude,
                    City = addressInfo.City,
                    IsDeleted = addressInfo.IsDeleted,
                    Id = addressInfo.Id,
                    Pincode = addressInfo.Pincode,
                    UpdateDate = addressInfo.UpdateDate

                };
                var stateName = _addressRepository.GetState(address.StateId);
                if (stateName != null) {
                    address.StateName = stateName.Name;
                }
                return address;
            }
            return null;

        }
        public List<State> GetState() {
            return _addressRepository.GetStateList().ToList();
        }
    }
}
