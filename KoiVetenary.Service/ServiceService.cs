using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Business;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace KoiVetenary.Service
{
    public interface IServiceService
    {
        Task<Data.Models.Service> GetServiceUsingOdata(int serviceId);
        Task<IQueryable<Data.Models.Service>> GetServicesUsingOdata();

        Task<IKoiVetenaryResult> GetServicesAsync();

        Task<IKoiVetenaryResult> GetServiceByIdAsync(int serviceId);

        Task<IKoiVetenaryResult> CreateService(Data.Models.Service service);

        Task<IKoiVetenaryResult> UpdateService(Data.Models.Service service);

        Task<IKoiVetenaryResult> DeleteService(int serviceId);

        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
    }
    public class ServiceService : IServiceService
    {

        private readonly UnitOfWork _unitOfWork;

        public ServiceService() 
        {
            _unitOfWork ??= new UnitOfWork();
        }
        //

        public static async Task<List<Data.Models.Service>> LoadServicesAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            // Đọc nội dung file JSON
            var jsonString = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON thành danh sách Service
            var services = JsonSerializer.Deserialize<List<Data.Models.Service>>(jsonString);

            return services ?? new List<Data.Models.Service>();
        }

        public async Task<IKoiVetenaryResult> CreateService(Data.Models.Service service)
        {
            try
            {
                int result = -1;

                var entity = _unitOfWork.ServiceRepository.GetById(service.ServiceId);

                if (entity != null)

                    return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, entity);

                else
                {
                    result = await _unitOfWork.ServiceRepository.CreateAsync(service);

                    if (result > 0)

                        return new KoiVetenaryResult (Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, service);

                    else

                        return new KoiVetenaryResult (Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, service);
                }
            }
            catch (System.Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> DeleteService(int serviceId)
        {
            try
            {
                var entity = _unitOfWork.ServiceRepository.GetById(serviceId);

                if (entity == null)

                    return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Data.Models.Service());

                else
                {
                    var result = await _unitOfWork.ServiceRepository.RemoveAsync(entity);

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

        public async Task<IKoiVetenaryResult> GetServiceByIdAsync(int serviceId)
        {
            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);

            if (service == null)

                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Data.Models.Service());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, service);
        }

        public async Task<IKoiVetenaryResult> GetServicesAsync()
        {
            var services = await _unitOfWork.ServiceRepository.GetAllAsync();

            if (services == null || !services.Any())
            
                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Data.Models.Service>());

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, services);
        }

        public async Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm)
        {
            var services = await _unitOfWork.ServiceRepository.SearchByKeyword(searchTerm);

            if (services == null || !services.Any())
            
                return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new List<Data.Models.Service>());           

            return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, services);
        }

        public async Task<IKoiVetenaryResult> UpdateService(Data.Models.Service service)
        {
            try
            {
                var entity = await _unitOfWork.ServiceRepository.GetByIdAsync(service.ServiceId);

                if (entity == null)
                {
                    return new KoiVetenaryResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Data.Models.Service());
                }
                else
                {
                    var result = await _unitOfWork.ServiceRepository.UpdateAsync(service);

                    if (result > 0)

                        return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, service);

                    else

                        return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, service);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        //For assignment 1
        public async Task<IQueryable<Data.Models.Service>> GetServicesUsingOdata()
        {
            var services = await LoadServicesAsync("SeedData/services.json");
            if (services == null)
                return Enumerable.Empty<Data.Models.Service>().AsQueryable();
            return services.AsQueryable();
        }
        public async Task<Data.Models.Service> GetServiceUsingOdata(int serviceId)
        {
            var services = await LoadServicesAsync("SeedData/services.json");
            if (services == null)
                return null;
            var service = services.FirstOrDefault(s => s.ServiceId == serviceId);
            return service;
        }
    }
}
