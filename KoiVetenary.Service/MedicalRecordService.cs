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
    public interface IMedicalRecordService
    {
        Task<IKoiVetenaryResult> GetMedicalRecordsAsync();
        Task<IKoiVetenaryResult> GetMedicalRecordByIdAsync(int? id);
        Task<IKoiVetenaryResult> CreateMedicalRecord(MedicalRecord medicalRecord);
        Task<IKoiVetenaryResult> UpdateMedicalRecord(MedicalRecord medicalRecord);
        Task<IKoiVetenaryResult> DeleteMedicalRecord(int? id);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }

    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly UnitOfWork _unitOfWork;

        public MedicalRecordService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                medicalRecord.CreatedDate = DateTime.Now;
                medicalRecord.CreatedBy = "Admin";
                medicalRecord.ModifiedBy = null;
                int result = await _unitOfWork.MedicalRecordRepository.CreateAsync(medicalRecord);
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> DeleteMedicalRecord(int? id)
        {
            try
            {
                var removedItem = await _unitOfWork.MedicalRecordRepository.GetByIdAsync((int)id);
                _unitOfWork.MedicalRecordRepository.PrepareRemove(removedItem);
                var result = await _unitOfWork.MedicalRecordRepository.SaveAsync();
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> GetMedicalRecordByIdAsync(int? id)
        {
            try
            {
                var result = await _unitOfWork.MedicalRecordRepository.GetByIdAsync((int)id);
                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> GetMedicalRecordsAsync()
        {
            try
            {
                var result = await _unitOfWork.MedicalRecordRepository.GetAllAsync();
                if (result != null)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            try
            {
                var result = await _unitOfWork.MedicalRecordRepository.SearchMedicalRecordsAsync(searchTerm);

                if (result.Any())
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, "No records found matching the search criteria");
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                var entity = await _unitOfWork.MedicalRecordRepository.GetByIdAsync(medicalRecord.RecordId);

                if (entity == null)
                {
                    return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new MedicalRecord());
                }
                else
                {
                    var animal = await _unitOfWork.AnimalRepository.GetByIdAsync((int)medicalRecord.AnimalId);
                    if (animal == null)
                    {
                        return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Animal is not found!");
                    }

                    medicalRecord.Animal = animal;
                    medicalRecord.UpdatedDate = DateTime.Now;
                    medicalRecord.CreatedDate = entity.CreatedDate;
                    medicalRecord.CreatedBy = entity.CreatedBy;
                    medicalRecord.ModifiedBy = "Admin";

                    var result = await _unitOfWork.MedicalRecordRepository.UpdateAsync(medicalRecord);

                    if (result > 0)

                        return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, medicalRecord);

                    else

                        return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, medicalRecord);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

    }
}


