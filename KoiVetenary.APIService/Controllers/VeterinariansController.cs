using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Business;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinariansController : ControllerBase
    {
        private readonly IVeterinarianService _veterinarian;

        public VeterinariansController(IVeterinarianService veterinarian)
        {
            _veterinarian = veterinarian;
        }

        // GET: api/Veterinarians
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetVeterinarians()
        {
            return await _veterinarian.GetVeterinariansAsync();
        }

        // GET: api/Veterinarians/5
        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> GetVeterinarian(int id)
        {
            return await _veterinarian.GetVeterinarianByIdAsync(id);
        }

        // PUT: api/Veterinarians/5
        [HttpPut("{id}")]
        public async Task<IKoiVetenaryResult> PutVeterinarian(int id, Veterinarian veterinarian)
        {
            return await _veterinarian.UpdateVeterinarian(veterinarian);
        }

        // POST: api/Veterinarians
        [HttpPost]
        public async Task<IKoiVetenaryResult> PostVeterinarian(Veterinarian veterinarian)
        {
            return await _veterinarian.CreateVeterinarian(veterinarian);
        }

        // DELETE: api/Veterinarians/5
        [HttpDelete("{id}")]
        public async Task<IKoiVetenaryResult> DeleteVeterinarian(int id)
        {
            return await _veterinarian.DeleteVeterinarian(id);
        }
    }
}
