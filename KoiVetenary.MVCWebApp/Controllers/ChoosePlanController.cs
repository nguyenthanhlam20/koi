using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.MVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class ChoosePlanController : Controller
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        //
        public async Task<IActionResult> Index(int page = 1, string searchQuery = "")
        {
            int pageSize = 5;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (services != null && services.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Data.Models.Service>>(services.Data.ToString());
                            var categories = await GetCategories();
                            foreach (var item in data)
                            {
                                item.Category = categories.FirstOrDefault(x => x.CategoryId == item.CategoryId);
                            }

                            //ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "Name", data.CategoryId);
                            if (!string.IsNullOrEmpty(searchQuery))
                            {
                                data = data.Where(s =>
                                    s.ServiceName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    s.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                    (s.Category != null && s.Category.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                                ).ToList();
                            }
                            int totalItems = data.Count();
                            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                            var items =  data
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .AsEnumerable();

                            var viewModel = new ServiceListViewModel
                            {
                                Services = items,
                                CurrentPage = page,
                                TotalPages = totalPages
                            };
                            return View(viewModel);
                        }
                    }
                }
            }
            return View(new List<Data.Models.Service>());
        }
        private static async Task<List<Category>> GetCategories()
        {
            var listCate = new List<Category>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Category"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var categories = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (categories != null && categories.Data != null)
                        {
                            listCate = JsonConvert.DeserializeObject<List<Category>>(categories.Data.ToString());
                        }
                    }
                }
            }
            return listCate;
        }
        //
        public List<Data.Models.Service> Paging(int currentPage = 1, List<Data.Models.Service> all = null)
        {
            const int pageSize = 6;
            CurrentPage = currentPage;
            var totalCount = all.Count;
            CalculateTotalPages(totalCount, pageSize);
            return all.Skip((CurrentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public void CalculateTotalPages(int totalCount, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
