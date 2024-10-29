using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService context)
        {
            _medicalRecordService = context;
        }

        [HttpGet]
        public async Task<IKoiVetenaryResult> GetMedicalRecordsAsync()
        {
            return await _medicalRecordService.GetMedicalRecordsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IKoiVetenaryResult> GetMedicalRecordByIdAsync(int? id)
        {
            return await _medicalRecordService.GetMedicalRecordByIdAsync(id);
        }

        [HttpPost]
        public async Task<IKoiVetenaryResult> Create([FromBody] MedicalRecord medicalRecord)
        {
            return await _medicalRecordService.CreateMedicalRecord(medicalRecord);
        }

        [HttpPut]
        public async Task<IKoiVetenaryResult> UpdateMedicalRecordAsync([FromBody] MedicalRecord medicalRecord)
        {
            return await _medicalRecordService.UpdateMedicalRecord(medicalRecord);
        }

        [HttpDelete("{id}")]
        public async Task<IKoiVetenaryResult> DeleteMedicalRecord([FromRoute] int id)
        {
            return await _medicalRecordService.DeleteMedicalRecord(id);
        }

        // GET: api/MedicalRecord/search?searchTerm={term}
        [HttpGet("search")]
        public async Task<IKoiVetenaryResult> SearchMedicalRecordsAsync([FromQuery] string searchTerm)
        {
            return await _medicalRecordService.SearchByKeyword(searchTerm);
        }
    }
}
