using KoiVetenary.Data.Base;
using KoiVetenary.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiVetenary.Data.Repositories;
public class PaymentRepository : GenericRepository<Payment>
{
    public async Task<Payment> GetByIdAsync(int id)
    {
        var entity = await _context.Payments.Include(a => a.Appointment).FirstOrDefaultAsync(a => a.PaymentId == id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
        return entity;
    }

    public async Task<List<Payment>> SearchAsync(int ownerId, string? searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            // Return all records if no search term is provided
            return await _context.Payments
                .Include(a => a.Appointment)
                .Where(x => x.Appointment.OwnerId == ownerId)
                .ToListAsync();
        }

        // Convert search term to lowercase for case-insensitive search
        searchTerm = searchTerm.ToLower();

        // Search across multiple fields using OR condition with case-insensitive comparison
        var query = _context.Payments
            .Include(a => a.Appointment)
            .Where(x => x.Appointment.OwnerId == ownerId)
            .Where(m => m.TotalAmount.ToString()!.Contains(searchTerm) ||
                        m.PayDate.ToString().Contains(searchTerm) ||
                      m.AppointmentId.ToString()!.Contains(searchTerm));

        return await query.ToListAsync();
    }
}
