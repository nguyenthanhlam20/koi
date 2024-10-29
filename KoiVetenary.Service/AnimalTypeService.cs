using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiVetenary.Service.DTO.Animal;

namespace KoiVetenary.Service
{

    public interface IAnimalTypeService
    {
        Task<IKoiVetenaryResult> GetAnimalTypesAsync();
        Task<IKoiVetenaryResult> GetAnimalTypesByIdAsync(int? typeId);
        Task<IKoiVetenaryResult> CreateAnimalTypes(AnimalType animalType);
        Task<IKoiVetenaryResult> UpdateAnimalTypes(AnimalType animalType);
        Task<IKoiVetenaryResult> DeleteAnimalTypes(int? typeId);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);

    }
    public class AnimalTypeService : IAnimalTypeService
    {
        private readonly UnitOfWork _unitOfWork;

        public AnimalTypeService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public Task<IKoiVetenaryResult> CreateAnimalTypes(AnimalType animalType)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> DeleteAnimalTypes(int? typeId)
        {
            throw new NotImplementedException();
        }

        public async Task<IKoiVetenaryResult> GetAnimalTypesAsync()
        {
            var animalTypes = await _unitOfWork.AnimalTypeRepository.GetAllAsync();

            if (animalTypes == null || !animalTypes.Any())

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<AnimalType>());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, animalTypes);
        }

        public Task<IKoiVetenaryResult> GetAnimalTypesByIdAsync(int? typeId)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<IKoiVetenaryResult> UpdateAnimalTypes(AnimalType animalType)
        {
            throw new NotImplementedException();
        }
    }
}
