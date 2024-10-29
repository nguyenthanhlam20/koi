using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Common;
using Newtonsoft.Json;
using KoiVetenary.Business;
using KoiVetenary.Service.DTO.Appointment;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService context)
        {
            _appointmentService = context;
        }

        // GET: Appointments
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAppoinentAsync()
        {
            return await _appointmentService.GetAppointmentsAsync();

            //var fA24_SE1716_PRN231_G3_KoiVetenaryContext = _appointmentService.Appointments.Include(a => a.Owner);
            //return View(await fA24_SE1716_PRN231_G3_KoiVetenaryContext.ToListAsync());
        }

        [HttpGet("Pending")]
        public async Task<IKoiVetenaryResult> GetPendingAppoinentAsync()
        {
            return await _appointmentService.GetPendingAppointmentsAsync();
        }

        [HttpGet("search")]
        public async Task<IKoiVetenaryResult> SearchAppointmentsAsync([FromQuery] String searchTerm)
        {
            return await _appointmentService.SearchByKeyword(searchTerm);
        }

        //GET: Appointments/Details/5
        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> GetAppointmentDetail( int? id)
        {
            return await _appointmentService.GetAppointmentByIdAsync(id);
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IKoiVetenaryResult> CreateAppointment([FromBody] Appointment appointment)
        {
            return await _appointmentService.CreateAppointment(appointment);
        }

        // PUT: Animals/Edit/5
        [HttpPut]
        public async Task<IKoiVetenaryResult> UpdateAnimalAsync([FromBody] Appointment appointment)
        {
            return await _appointmentService.UpdateAppointment(appointment);
        }

        // DELETE: Animals/Delete/5
        [HttpDelete("{id}")]
        public async Task<IKoiVetenaryResult> DeleteAnimal([FromRoute] int id)
        {
            return await _appointmentService.DeleteAppointment(id);
        }
    }
}
