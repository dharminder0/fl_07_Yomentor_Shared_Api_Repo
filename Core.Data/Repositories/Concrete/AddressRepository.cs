using Core.Business.Entities.DataModels;
using Core.Common.Data;
using Core.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Repositories.Concrete {
    public class AddressRepository :DataRepository<Address>,IAddressRepository {
        public int UpsertAddress(Address address) {
            var sql = @"
        IF EXISTS (SELECT 1 FROM Address WHERE Id = @Id)
        BEGIN
            UPDATE Address
            SET
                UserId = @UserId,
                Address1 = @Address1,
                Address2 = @Address2,
                City = @City,
                StateId = @StateId,
                Pincode = @Pincode,
                Latitude = @Latitude,
                Longitude = @Longitude,
                UpdateDate = GETDATE(),
                IsDeleted = @IsDeleted
            WHERE Id = @Id;

            SELECT Id FROM Address WHERE Id = @Id;
        END
        ELSE
        BEGIN
            INSERT INTO Address
            (
                UserId,
                Address1,
                Address2,
                City,
                StateId,
                Pincode,
                Latitude,
                Longitude,
                CreateDate,
                UpdateDate,
                IsDeleted
            )
            VALUES
            (
                @UserId,
                @Address1,
                @Address2,
                @City,
                @StateId,
                @Pincode,
                @Latitude,
                @Longitude,
                GETDATE(),
                GETDATE(),
                @IsDeleted
            );

            SELECT SCOPE_IDENTITY();
        END
    ";

            return ExecuteScalar<int>(sql, new {
                Id = address.Id,
                UserId = address.UserId,
                Address1 = address.Address1,
                Address2 = address.Address2,
                City = address.City,
                StateId = address.StateId,
                Pincode = address.Pincode,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                IsDeleted = address.IsDeleted
            });
        }

       public Address GetUserAddress(int userId) {
            var sql = @" select * from Address  where userId=@userId and IsDeleted=0 ";
            return  QueryFirst<Address>(sql, new { userId });
        }
        public IEnumerable<State> GetStateList() {
            var sql = @" select * from state ";
            return  Query<State>(sql);
        }
        public State  GetState(int Id) {
            var sql = " select  name  from state where id=@Id";
            return QueryFirst<State>(sql, new { Id});
        }
    } 
}
