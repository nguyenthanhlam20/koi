using KoiVetenary.Data.Base;
using KoiVetenary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Data.Repositories
{
    public class VeterinarianRepository : GenericRepository<Veterinarian>
    {
        public VeterinarianRepository() {}
        public VeterinarianRepository(FA24_SE1716_PRN231_G3_KoiVetenaryContext context) => _context = context;
    }
}
