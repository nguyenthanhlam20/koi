using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using KoiVetenary.Service.DTO.Animal;
using KoiVetenary.Service.DTO.Appointment;
using LinqKit;
using System;
using System.Data.Entity;
using System.Linq;


namespace KoiVetenary.Service
{
    public interface IAppointmentService
    {
        Task<IKoiVetenaryResult> GetAppointmentsAsync();
        Task<IKoiVetenaryResult> GetPendingAppointmentsAsync();
        Task<IKoiVetenaryResult> GetAppointmentByIdAsync(int? id);
        Task<IKoiVetenaryResult> CreateAppointment(Appointment appointment);
        Task<IKoiVetenaryResult> UpdateAppointment(Appointment appointment);
        Task<IKoiVetenaryResult> DeleteAppointment(int? id);
        Task<IKoiVetenaryResult> SearchByKeyword(string? searchTerm);
        Task<IKoiVetenaryResult> SearchByMultipleKeyword(AppointmentSearchCriteria searchTerm);
    }
    public class AppointmentService : IAppointmentService
    {
        private readonly UnitOfWork _unitOfWork; 

        public AppointmentService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IKoiVetenaryResult> CreateAppointment(Appointment appointment)
        {
            try
            {
                var owner = _unitOfWork.OwnerRepository.GetById((int)appointment.OwnerId);
                if (owner == null) {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Owner not found");
                }
                if (IsPastAppointment(appointment.AppointmentDate, appointment.AppointmentTime))
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Appointment Date and Time cannot in the past");
                }
                appointment.IsPaid = false;  
                appointment.TotalCost = 0;    
                appointment.TotalEstimatedDuration = 0;
                appointment.CreatedDate = DateTime.Now;
                appointment.UpdatedDate = DateTime.Now;
                appointment.CreatedBy = owner.FirstName + owner.LastName;
                appointment.ModifiedBy = owner.FirstName + owner.LastName;
                int result = await _unitOfWork.AppointmentRepository.CreateAsync(appointment);
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

        public async Task<IKoiVetenaryResult> DeleteAppointment(int? id)
        {
            try
            {
                // Fetch the appointment along with its details
                var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)id);

                if (appointment == null)
                {
                    return new KoiVetenaryResult(Const.FAIL_DELETE_CODE, "Appointment not found.");
                }

                // Remove all related appointment details first
                var appointmentDetails = await _unitOfWork.AppointmentDetailRepository.GetDetailsByAppointmentIdAsync((int)id);
                foreach (var detail in appointmentDetails)
                {
                    _unitOfWork.AppointmentDetailRepository.PrepareRemove(detail);
                }
                var appResultDetail = await _unitOfWork.AppointmentDetailRepository.SaveAsync();

                // Remove the appointment itself
                _unitOfWork.AppointmentRepository.PrepareRemove(appointment);

                // Save all changes
                var appResult = await _unitOfWork.AppointmentRepository.SaveAsync();

                if (appResult > 0)
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
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }


        public async Task<IKoiVetenaryResult> GetAppointmentByIdAsync(int? id)
        {
            try
            {
                
                var result = await _unitOfWork.AppointmentRepository.GetByIdAsync((int)id);

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

        public async Task<IKoiVetenaryResult> GetAppointmentsAsync()
        {
            try
            {

                var result = await _unitOfWork.AppointmentRepository.GetAllAsync();

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

        public async Task<IKoiVetenaryResult> GetPendingAppointmentsAsync()
        {
            try
            {

                var result = await _unitOfWork.AppointmentRepository.GetAllPendingAsync();

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
                var result = await _unitOfWork.AppointmentRepository.SearchAsync(searchTerm);

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

        public async Task<IKoiVetenaryResult> SearchByMultipleKeyword(AppointmentSearchCriteria searchCriteria)
        {
            try
            {
                var query = _unitOfWork.AppointmentRepository.GetQueryable();

                // Initialize a predicate for OR conditions
                var predicate = PredicateBuilder.New<Appointment>(false); // Start with false for OR conditions

                // Filter by Contact Email
                if (!string.IsNullOrWhiteSpace(searchCriteria.ContactEmail))
                {
                    predicate = predicate.Or(a => a.ContactEmail.Contains(searchCriteria.ContactEmail));
                }

                // Filter by Contact Phone
                if (!string.IsNullOrWhiteSpace(searchCriteria.ContactPhone))
                {
                    predicate = predicate.Or(a => a.ContactPhone.Contains(searchCriteria.ContactPhone));
                }

                // Filter by Status
                if (!string.IsNullOrWhiteSpace(searchCriteria.Status))
                {
                    predicate = predicate.Or(a => a.Status.Contains(searchCriteria.Status));
                }

                // Add total cost filtering
                if (searchCriteria.TotalCostFrom.HasValue)
                {
                    predicate = predicate.And(a => a.TotalCost >= searchCriteria.TotalCostFrom.Value);
                }

                if (searchCriteria.TotalCostTo.HasValue)
                {
                    predicate = predicate.And(a => a.TotalCost <= searchCriteria.TotalCostTo.Value);
                }

                // Apply the predicate to the query
                query = query.Where(predicate);

                var animals = await query.Include(a => a.Owner)
                    .ToListAsync();

                if (animals.Any())
                {
                    return new KoiVetenaryResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, animals);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, "No appointments found matching the criteria.");
                }
            }
            catch (Exception ex)
            {
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IKoiVetenaryResult> UpdateAppointment(Appointment appointment)
        {
            try
            {
                var owner = _unitOfWork.OwnerRepository.GetById((int)appointment.OwnerId);
                if (owner == null)
                {
                    return new KoiVetenaryResult(Const.ERROR_EXCEPTION, "Owner not found");
                }

                var existed = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointment.AppointmentId);
                if (existed == null)
                {
                    return new KoiVetenaryResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                }
            
                existed.AppointmentDate = appointment.AppointmentDate;
                existed.AppointmentTime = appointment.AppointmentTime;
                existed.Status = appointment.Status;
                existed.Notes = appointment.Notes;
                existed.TotalEstimatedDuration = appointment.TotalEstimatedDuration;
                existed.TotalCost = appointment.TotalCost;
                existed.ModifiedBy = "admin";
                existed.UpdatedDate = DateTime.Now;
                existed.Owner = owner;

                _unitOfWork.AppointmentRepository.PrepareUpdate(existed);

                // Save the changes asynchronously
                var result = await _unitOfWork.AppointmentRepository.SaveAsync();

                // Check the result and return appropriate response
                if (result > 0)
                {
                    return new KoiVetenaryResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG);
                }
                else
                {
                    return new KoiVetenaryResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error result
                return new KoiVetenaryResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public bool IsPastAppointment(DateTime? appointmentDate, TimeSpan? appointmentTime)
        {
            // Check if either the date or time is null
            if (!appointmentDate.HasValue || !appointmentTime.HasValue)
            {
                return false; // or handle this case as you see fit
            }

            // Combine the date and time into a single DateTime object
            DateTime combinedDateTime = appointmentDate.Value.Date.Add(appointmentTime.Value);

            // Check if the combined date and time is in the past
            return combinedDateTime < DateTime.Now;
        }
    }
}
