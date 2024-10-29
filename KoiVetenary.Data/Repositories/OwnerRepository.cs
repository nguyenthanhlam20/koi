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
    public class OwnerRepository : GenericRepository<Owner>
    {
        public OwnerRepository() { }

        public async Task<IEnumerable<Owner>> GetAllWithoutAppointmentsAsync()
        {
            return await _context.Owners
                .Select(o => new Owner
                {
                    OwnerId = o.OwnerId,
                    FirstName = o.FirstName,
                    LastName = o.LastName
                })
                .ToListAsync();
        }
    }
   
}
