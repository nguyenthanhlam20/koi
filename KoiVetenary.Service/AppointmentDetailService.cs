using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using KoiVetenary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoiVetenary.Common;

namespace KoiVetenary.Service
{
    public interface IAppointmentDetailService
    {
        Task<IKoiVetenaryResult> GetAppointmentDetailsAsync();

        Task<IKoiVetenaryResult> UpdateDetailAppointmentServiceID(int appointmentId, int serviceId);
        Task<IKoiVetenaryResult> UpdateDetailAppointmentVeteID(int appointmentId, int veteId);
        Task<IKoiVetenaryResult> CreateAppointmentDetailAsync(AppointmentDetail appointment);
    }
    public class AppointmentDetailService : IAppointmentDetailService
    {

        private readonly UnitOfWork _unitOfWork;

        public AppointmentDetailService()
        {
            _unitOfWork ??= new UnitOfWork();
        } 

        public async Task<IKoiVetenaryResult> GetAppointmentDetailsAsync()
        {
            try
            {
                var result = await _unitOfWork.AppointmentDetailRepository.GetAllAsync();

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

        public async Task<IKoiVetenaryResult> UpdateDetailAppointmentServiceID(int appointmentId, int serviceId)
        {
            try
            {
                var detail = await _unitOfWork.AppointmentDetailRepository.GetByIdAsync(appointmentId);
                if (detail != null)
                {
                    var service = await _unitOfWork.ServiceRepository.GetByIdAsync(serviceId);
                    var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)detail.AppointmentId);
                    appointment.TotalCost += service.BasePrice;
                    appointment.TotalEstimatedDuration += service.Duration;
                    detail.Service = service;
                    await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
                    int result = await _unitOfWork.AppointmentDetailRepository.UpdateAsync(detail);
                    if (result > 0)
                    {
                        return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> CreateAppointmentDetailAsync(AppointmentDetail appointment)
        {
            try
            {
                var pendingApp = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)appointment.AppointmentId);
                if (pendingApp == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Appointment not found");
                }
                else
                {
                    if (!pendingApp.Status.Equals(AppointmentStatus.Pending))
                    {
                        return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Appointment must be PENDING to add appointment detail");
                    }
                    else
                    {
                        appointment.CreatedDate = DateTime.Now;
                        appointment.UpdatedDate = DateTime.Now;
                        appointment.CreatedBy = pendingApp.Owner.FirstName + pendingApp.Owner.LastName;
                        appointment.ModifiedBy = pendingApp.Owner.FirstName + pendingApp.Owner.LastName;
                        int result = await _unitOfWork.AppointmentDetailRepository.CreateAsync(appointment);
                        if (result > 0)
                        {
                            return new KoiVetenaryResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, result);
                        }
                        else
                        {
                            return new KoiVetenaryResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> UpdateDetailAppointmentVeteID(int appointmentId, int veteId)
        {

           try
            {
                var appointment = await _unitOfWork.AppointmentDetailRepository.GetByIdAsync(appointmentId);
                if (appointment != null)
                {
                    appointment.VeterinarianId = veteId;
                    int result = await _unitOfWork.AppointmentDetailRepository.UpdateAsync(appointment);
                    if (result > 0)
                    {
                        return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                    }
                    else
                    {
                        return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                    }
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
