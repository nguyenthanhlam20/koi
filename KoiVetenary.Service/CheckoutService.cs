using KoiVetenary.Data;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Service
{
    public interface ICheckoutService
    {
        public Task<Appointment> Checkout(int appointmentId);
        public Task<Appointment> CreatePayment(int appointmentId, Payment transaction);
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly UnitOfWork _unitOfWork;
        public CheckoutService()
        {
            _unitOfWork ??= new UnitOfWork();

        }
        public async Task<Appointment> Checkout(int appointmentId)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
            return appointment;
        }

        public async Task<Appointment> CreatePayment(int appointmentId, Payment transaction)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);

            // Check if Payments collection is null, and initialize if necessary
            if (appointment.Payments == null)
            {
                appointment.Payments = new List<Payment>();
            }

            // Add the transaction (Payment object) to the Payments collection
            appointment.Payments.Add(transaction);

            // Update the appointment status
            appointment.IsPaid = true;

            // Update the appointment in the repository
            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);

            // Return the updated appointment
            return appointment; ;
        }
    }
}
