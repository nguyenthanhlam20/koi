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
using KoiVetenary.Business;
using Newtonsoft.Json;
using System.Text;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _service;
        private readonly ICategoryService _category;

        public ServicesController(IServiceService service, ICategoryService category)
        {
            _service = service;
            _category = category;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
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
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Data.Models.Service>());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var service = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (service != null && service.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Data.Models.Service>(service.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new Data.Models.Service());
        }

        //GET: Services/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "Name");
            return View();
        }

        //POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,ServiceName,Description,Duration,BasePrice,CategoryId,IsActive,RequiredEquipment,SpecialInstructions,ServiceImg,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Data.Models.Service service)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.API_Endpoint + "Services", service))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                                saveStatus = true;
                            else
                                saveStatus = false;
                        }
                    }
                }
            }
            if(saveStatus)
                return RedirectToAction(nameof(Index));
            else
            {
                ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "Name", service.CategoryId);
                return View(service);
            }           
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

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var service = new Data.Models.Service();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (serviceResult != null && serviceResult.Data != null)
                        {
                            service = JsonConvert.DeserializeObject<Data.Models.Service>(serviceResult.Data.ToString());
                        }
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        //POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,ServiceName,Description,Duration,BasePrice,CategoryId,IsActive,RequiredEquipment,SpecialInstructions,ServiceImg,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Data.Models.Service service)
        {

            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.API_Endpoint + "Services/" + id, service))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                                saveStatus = true;
                            else
                                saveStatus = false;
                        }
                    }
                }
            }
            if (saveStatus)
                return RedirectToAction(nameof(Index));
            else
            {
                ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "Name", service.CategoryId);
                return View(service);
            }
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var service = new Data.Models.Service();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (serviceResult != null && serviceResult.Data != null)
                        {
                            service = JsonConvert.DeserializeObject<Data.Models.Service>(serviceResult.Data.ToString());
                            return View(service);
                        }
                    }
                }
            }
            return View(service);
        }


        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(Const.API_Endpoint + "Services/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Delete), new { id = id });
        }

        
    }
}
