using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using KoiVetenary.Business;
using KoiVetenary.Common;
using Newtonsoft.Json;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class VeterinariansController : Controller
    {

        public VeterinariansController()
        {
            
        }

        // GET: Veterinarians
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Veterinarians"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (services != null && services.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Veterinarian>>(services.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Veterinarian>());
        }

        // GET: Veterinarians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Veterinarians/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var service = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (service != null && service.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<Veterinarian>(service.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new Veterinarian());
        }

        // GET: Veterinarians/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Veterinarians/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VeterinarianId,FirstName,LastName,Specialization,LicenseNumber,YearsOfExperience,Phone,Email,HireDate,IsActive,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Veterinarian veterinarian)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.API_Endpoint + "Veterinarians", veterinarian))
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
                return View(veterinarian);
            }
        }

        // GET: Veterinarians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var veterinarian = new Veterinarian();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Veterinarians/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (serviceResult != null && serviceResult.Data != null)
                        {
                            veterinarian = JsonConvert.DeserializeObject<Veterinarian>(serviceResult.Data.ToString());
                        }
                    }
                }
            }
            return View(veterinarian);
        }

        // POST: Veterinarians/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VeterinarianId,FirstName,LastName,Specialization,LicenseNumber,YearsOfExperience,Phone,Email,HireDate,IsActive,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] Veterinarian veterinarian)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PutAsJsonAsync(Const.API_Endpoint + "Veterinarians/" + id, veterinarian))
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
                return View(veterinarian);
            }
        }

        // GET: Veterinarians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var service = new Veterinarian();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Veterinarians/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (serviceResult != null && serviceResult.Data != null)
                        {
                            service = JsonConvert.DeserializeObject<Veterinarian>(serviceResult.Data.ToString());
                            return View(service);
                        }
                    }
                }
            }
            return View(service);
        }

        // POST: Veterinarians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(Const.API_Endpoint + "Veterinarians/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return RedirectToAction(nameof(Delete), new { id = id });
        }

        //
        // GET: Veterinarians/GetVeterinarians to update appoinment
        public async Task<IActionResult> ChooseVete()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Veterinarians"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (services != null && services.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<Veterinarian>>(services.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }
            return View(new List<Veterinarian>());
        }
    }
}
