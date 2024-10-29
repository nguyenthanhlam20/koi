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
    public class AppointmentDetailRepository : GenericRepository<AppointmentDetail>
    {
        public AppointmentDetailRepository() { }

        public async Task<int> CreateAsync(AppointmentDetail detail)
        {
            _context.Add(detail);
            await _context.SaveChangesAsync();
            return detail.AppointmentDetailId;
        }

        //update serviceId in AppointmentDetail
        public async Task<bool> UpdateServiceId(int appointmentId, int serviceId)
        {
            try
            {
                var appointmentDetail = await _context.AppointmentDetails.FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
                if (appointmentDetail != null)
                {
                    appointmentDetail.ServiceId = serviceId;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateVeteId(int appointmentId, int veteId)
        {
            try
            {
                var appointmentDetail = await _context.AppointmentDetails.FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
                if (appointmentDetail != null)
                {
                    appointmentDetail.VeterinarianId = veteId;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AppointmentDetail> GetByIdAsync(int id)
        {
            var entity = await _context.AppointmentDetails.Include(a => a.Appointment).FirstOrDefaultAsync(a => a.AppointmentDetailId == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<IEnumerable<AppointmentDetail>> GetDetailsByAppointmentIdAsync(int appointmentId)
        {
            return await _context.AppointmentDetails
                                 .Where(ad => ad.AppointmentId == appointmentId)
                                 .ToListAsync();
        }

    }


}
