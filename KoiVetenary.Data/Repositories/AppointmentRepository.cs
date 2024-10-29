using KoiVetenary.Common;
using KoiVetenary.Data.Base;
using KoiVetenary.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Data.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>
    {
        public AppointmentRepository() { }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.Include(a => a.Owner).ToListAsync();
        }

        public async Task<List<Appointment>> GetAllPendingAsync()
        {
            return await _context.Appointments.Include(a => a.Owner).Where(a => a.Status.Equals(AppointmentStatus.Pending)).ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            var entity = await _context.Appointments.Include(a => a.Owner).FirstOrDefaultAsync(a => a.AppointmentId == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }
        public async Task<Appointment> GetByIdWithTrackingAsync(int id)
        {
            var entity = await _context.Appointments.Include(a => a.Owner)
                .AsTracking()
                .FirstOrDefaultAsync(a => a.AppointmentId == id);
            return entity;
        }


        public async Task<List<Appointment>> SearchAsync(string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all records if no search term is provided
                return await _context.Appointments.Include(a => a.Owner).ToListAsync();
            }

            // Convert search term to lowercase for case-insensitive search
            searchTerm = searchTerm.ToLower();

            // Search across multiple fields using OR condition with case-insensitive comparison
            var query = _context.Appointments.Include(a => a.Owner)
                                               .Where(m => m.Status.ToLower().Contains(searchTerm) ||
                                                           m.SpecialRequests.ToLower().Contains(searchTerm) ||
                                                           m.Owner.LastName.ToLower().Contains(searchTerm) ||
                                                           m.Owner.FirstName.ToLower().Contains(searchTerm) ||
                                                           m.ContactEmail.ToLower().Contains(searchTerm) ||
                                                           m.ContactPhone.ToLower().Contains(searchTerm));

            return await query.ToListAsync();
        }

        public IQueryable<Appointment> GetQueryable()
        {
            return _context.Appointments
                           .Include(a => a.Owner)
                           .AsQueryable();
        }
    }
}
