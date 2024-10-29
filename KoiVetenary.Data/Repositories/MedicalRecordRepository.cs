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
    public class MedicalRecordRepository : GenericRepository<MedicalRecord>
    {
        public MedicalRecordRepository() { }
        public MedicalRecordRepository(FA24_SE1716_PRN231_G3_KoiVetenaryContext context) => _context = context;

        public async Task<List<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords.Include(a => a.Animal).ToListAsync();
        }

        public async Task<MedicalRecord> GetByIdAsync(int id)
        {
            var entity = await _context.MedicalRecords.Include(a => a.Animal).FirstOrDefaultAsync(a => a.RecordId == id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<List<MedicalRecord>> SearchMedicalRecordsAsync(string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                // Return all records if no search term is provided
                return await _context.MedicalRecords.Include(a => a.Animal).ToListAsync();
            }

            // Convert search term to lowercase for case-insensitive search
            searchTerm = searchTerm.ToLower();

            // Search across multiple fields using OR condition with case-insensitive comparison
            var query = _context.MedicalRecords.Include(a => a.Animal)
                                               .Where(m => m.Symptoms.ToLower().Contains(searchTerm) ||
                                                           m.Diagnosis.ToLower().Contains(searchTerm) ||
                                                           m.Treatment.ToLower().Contains(searchTerm) ||
                                                           m.Medications.ToLower().Contains(searchTerm) ||
                                                           m.LabResults.ToLower().Contains(searchTerm) ||
                                                           m.VetNotes.ToLower().Contains(searchTerm));

            return await query.ToListAsync();
        }

    }
}
