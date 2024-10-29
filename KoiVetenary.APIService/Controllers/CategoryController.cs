using KoiVetenary.Business;
using KoiVetenary.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KoiVetenary.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _category;

        public CategoryController(ICategoryService category)
        {
            _category = category;
        }

        [HttpGet]
        public async Task<IKoiVetenaryResult> GetCategories()
        {
            return await _category.GetCategoriesAsync();
        }
    }
}
