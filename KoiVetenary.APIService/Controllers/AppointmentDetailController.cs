using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentDetailController : ControllerBase
    {
        private readonly IAppointmentDetailService _appointmentDetailService;

        public AppointmentDetailController(IAppointmentDetailService context)
        {
            _appointmentDetailService = context;
        }

        // GET: Appointments
        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAppoinentAsync()
        {
            return await _appointmentDetailService.GetAppointmentDetailsAsync();

            //var fA24_SE1716_PRN231_G3_KoiVetenaryContext = _appointmentService.Appointments.Include(a => a.Owner);
            //return View(await fA24_SE1716_PRN231_G3_KoiVetenaryContext.ToListAsync());
        }

        [HttpPut("update-service/{appointmentId}/{serviceId}")]
        public async Task<IKoiVetenaryResult> UpdateDetailAppServiceId(int appointmentId, int serviceId)
        {
            return await _appointmentDetailService.UpdateDetailAppointmentServiceID(appointmentId, serviceId);
        }

        [HttpPut("update-vete/{appointmentId}/{veteId}")]
        public async Task<IKoiVetenaryResult> UpdateDetailAppVeteId(int appointmentId, int veteId)
        {
            return await _appointmentDetailService.UpdateDetailAppointmentVeteID(appointmentId, veteId);
        }

        [HttpPost]
        public async Task<IKoiVetenaryResult> Create([FromBody] AppointmentDetail appointment)
        {
            return await _appointmentDetailService.CreateAppointmentDetailAsync(appointment);
        }

        

    }
}
