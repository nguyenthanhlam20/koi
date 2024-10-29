using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface IOwnerService
    {
        Task<IKoiVetenaryResult> GetOwnersAsync();

        Task<IKoiVetenaryResult> GetOwnerById(int id);

        Task<IKoiVetenaryResult> CreateOwner(Owner owner);

        Task<IKoiVetenaryResult> UpdateOwner(Owner owner);

        Task<IKoiVetenaryResult> DeleteOwner(int id);

    }
    public class OwnerService : IOwnerService
    {
        private readonly UnitOfWork _unitOfWork;

        public OwnerService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateOwner(Owner owner)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> DeleteOwner(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> GetOwnersAsync()
        {
            var owners = await _unitOfWork.OwnerRepository.GetAllAsync();

            if (owners == null || !owners.Any())

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Owner>());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, owners);
        }

        public Task<IKoiVetenaryResult> GetOwnerById(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> UpdateOwner(Owner owner)
        {
            throw new NotImplementedException();
        }
    }
}
