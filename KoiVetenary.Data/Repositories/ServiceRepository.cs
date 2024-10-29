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
    public class ServiceRepository : GenericRepository<Service>
    {
        public ServiceRepository() {}
        public ServiceRepository(FA24_SE1716_PRN231_G3_KoiVetenaryContext context) => _context = context;

        //

        public async Task<IEnumerable<Models.Service>> SearchByKeyword (string? searchTerm)

        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Services
                       .Include(s => s.Category)
                       .ToListAsync();
            }

            searchTerm = searchTerm.ToLower();

            var query = _context.Services
                        .Include(s => s.Category)
                        .Where(s =>
                            (s.ServiceName != null && s.ServiceName.ToLower().Contains(searchTerm)) ||
                            (s.Description != null && s.Description.ToLower().Contains(searchTerm)) ||
                            (s.Category != null && s.Category.Name != null && s.Category.Name.ToLower().Contains(searchTerm))
                        );

            return await query.ToListAsync();
        }


    }
}
