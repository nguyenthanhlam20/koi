using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Business;
using KoiVetenary.Service;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnerService _context;

        public OwnersController(IOwnerService context)
        {
            _context = context;
        }

        // GET: api/Owners
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetOwners()
        {
            return await _context.GetOwnersAsync();
        }
    }
}
