
using KoiVetenary.Service;
using KoiVetenary.Business;
using KoiVetenary.Data.Models;
using Microsoft.AspNetCore.Mvc;
using KoiVetenary.Service.DTO.Animal;

namespace KoiVetenary.APIService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimalTypesController : ControllerBase
    {
        private readonly IAnimalTypeService _animalTypeService;

        public AnimalTypesController(IAnimalTypeService animalTypeService)
        {
            _animalTypeService = animalTypeService;
        }

        [HttpGet]
        public async Task<IKoiVetenaryResult> GetAnimalTypesAsync()
        {
            return await _animalTypeService.GetAnimalTypesAsync();
        }
    }
}
