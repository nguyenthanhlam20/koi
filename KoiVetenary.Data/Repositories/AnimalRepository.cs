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
    public class AnimalRepository : GenericRepository<Animal>
    {
        public AnimalRepository() { }

        public async Task<List<Animal>> GetAllAsync()
        {
            return await _context.Animals.Include(a => a.Owner).Include(b => b.Type).ToListAsync();
        }

        public async Task<Animal> GetByIdAsync(int id)
        {
            var entity = await _context.Animals.Include(a => a.Owner).Include(b => b.Type).FirstOrDefaultAsync(a => a.AnimalId == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public IQueryable<Animal> GetQueryable()
        {
            return _context.Animals
                           .Include(a => a.Owner)
                           .Include(a => a.Type)
                           .AsQueryable();
        }
    }
}
