using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface IVeterinarianService
    {
        Task<IKoiVetenaryResult> GetVeterinariansAsync(); //veterinarians

        Task<IKoiVetenaryResult> GetVeterinarianByIdAsync(int veterinarianId);

        Task<IKoiVetenaryResult> CreateVeterinarian(Veterinarian veterinarian);

        Task<IKoiVetenaryResult> UpdateVeterinarian(Veterinarian veterinarian);

        Task<IKoiVetenaryResult> DeleteVeterinarian(int veterinarianId);

        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }
    public class VeterinarianService : IVeterinarianService
    {
        private readonly UnitOfWork _unitOfWork;

        public VeterinarianService()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IKoiVetenaryResult> CreateVeterinarian(Veterinarian veterinarian)
        {
            try
            {
                int result = -1;

                var entity = _unitOfWork.VeterinarianRepository.GetById(veterinarian.VeterinarianId);

                if (entity != null)

                    return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, entity);

                else
                {
                    result = await _unitOfWork.VeterinarianRepository.CreateAsync(veterinarian);

                    if (result > 0)

                        return new KoiVetenaryResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, veterinarian);

                    else

                        return new KoiVetenaryResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, veterinarian);
                }
            }
            catch (System.Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> DeleteVeterinarian(int veterinarianId)
        {
            try
            {
                var entity = _unitOfWork.VeterinarianRepository.GetById(veterinarianId);

                if (entity == null)

                    return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Veterinarian());

                else
                {
                    var result = await _unitOfWork.VeterinarianRepository.RemoveAsync(entity);

                    if (result)

                        return new KoiVetenaryResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, entity);
                    else

                        return new KoiVetenaryResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, entity);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IKoiVetenaryResult> GetVeterinarianByIdAsync(int veterinarianId)
        {
            var veterinarian = await _unitOfWork.VeterinarianRepository.GetByIdAsync(veterinarianId);

            if (veterinarian == null)

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Veterinarian());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, veterinarian);
        }

        public async Task<IKoiVetenaryResult> GetVeterinariansAsync()
        {
            var veterinarians = await _unitOfWork.VeterinarianRepository.GetAllAsync();

            if (veterinarians == null || !veterinarians.Any())

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Veterinarian>());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, veterinarians);
        }

        public Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> UpdateVeterinarian(Veterinarian veterinarian)
        {
            try
            {
                var entity = await _unitOfWork.VeterinarianRepository.GetByIdAsync(veterinarian.VeterinarianId);

                if (entity == null)
                {
                    return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Veterinarian());
                }
                else
                {
                    var result = await _unitOfWork.VeterinarianRepository.UpdateAsync(veterinarian);

                    if (result > 0)

                        return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, veterinarian);

                    else

                        return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, veterinarian);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
