using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using KoiVetenary.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KoiVetenary.MVCWebApp.Controllers
{
    public class MedicalRecordsController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordsController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        // GET: MedicalRecord
        public async Task<IActionResult> Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "MedicalRecord"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                            if (services != null && services.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<List<MedicalRecord>>(services.Data.ToString());
                                return View(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return View(new List<MedicalRecord>());
        }

        // GET: MedicalRecord/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "MedicalRecord/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var service = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                            if (service != null && service.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<MedicalRecord>(service.Data.ToString());
                                return View(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return View(new MedicalRecord());
        }

        // GET: MedicalRecord/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["AnimalId"] = new SelectList(await this.GetAnimals(), "AnimalId", "Name");
            return View();
        }

        private async Task<List<Animal>> GetAnimals()
        {
            var listAnimals = new List<Animal>();
            //
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Animals"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var owners = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                        if (owners != null && owners.Data != null)
                        {
                            listAnimals = JsonConvert.DeserializeObject<List<Animal>>(owners.Data.ToString());
                        }
                    }
                }
            }
            return listAnimals;
        }

        // POST: MedicalRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecordId,AnimalId,RecordDate,Symptoms,Diagnosis,Treatment,Medications,LabResults,VetNotes,FollowUpRequired,FollowUpDate,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] MedicalRecord medicalRecord)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.PostAsJsonAsync(Const.API_Endpoint + "MedicalRecord", medicalRecord))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                            if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                            {
                                saveStatus = true;
                            }
                            else
                            {
                                saveStatus = false;
                            }
                        }
                    }
                }
            }
            if (saveStatus)
                return RedirectToAction(nameof(Index));
            else
            {
                return View(medicalRecord);
            }
        }

        // GET: MedicalRecord/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var medicalRecord = new MedicalRecord();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "MedicalRecord/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                            if (serviceResult != null && serviceResult.Data != null)
                            {
                                medicalRecord = JsonConvert.DeserializeObject<MedicalRecord>(serviceResult.Data.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
            ViewData["AnimalId"] = new SelectList(await this.GetAnimals(), "AnimalId", "Name");
            return View(medicalRecord);
        }

        // POST: MedicalRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecordId,AnimalId,RecordDate,Symptoms,Diagnosis,Treatment,Medications,LabResults,VetNotes,FollowUpRequired,FollowUpDate,CreatedBy,ModifiedBy,CreatedDate,UpdatedDate")] MedicalRecord medicalRecord)
        {
            bool saveStatus = false;

            if (ModelState.IsValid)
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PutAsJsonAsync(Const.API_Endpoint + "MedicalRecord/", medicalRecord))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                var result = JsonConvert.DeserializeObject<KoiVetenaryResult>(content);
                                if (result != null && result.Status == Const.SUCCESS_CREATE_CODE)
                                {
                                    saveStatus = true;
                                }
                                else
                                {
                                    saveStatus = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                }
            }

            if (saveStatus)
                return RedirectToAction(nameof(Index));
            else
                return View(medicalRecord);
        }

        // GET: MedicalRecord/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var medicalRecord = new MedicalRecord();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "MedicalRecord/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var serviceResult = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                            if (serviceResult != null && serviceResult.Data != null)
                            {
                                medicalRecord = JsonConvert.DeserializeObject<MedicalRecord>(serviceResult.Data.ToString());
                                return View(medicalRecord);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
            ViewData["AnimalId"] = new SelectList(await this.GetAnimals(), "AnimalId", "Name");
            return View(medicalRecord);
        }

        // POST: MedicalRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool deleteStatus = false;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.DeleteAsync(Const.API_Endpoint + "MedicalRecord/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            deleteStatus = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            if (deleteStatus)
                return RedirectToAction(nameof(Index));
            else
                return RedirectToAction(nameof(Delete), new { id });
        }

        // GET: MedicalRecord/ChooseRecord for selection purposes
        public async Task<IActionResult> ChooseRecord()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Const.API_Endpoint + "MedicalRecord"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var services = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                            if (services != null && services.Data != null)
                            {
                                var data = JsonConvert.DeserializeObject<List<MedicalRecord>>(services.Data.ToString());
                                return View(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return View(new List<MedicalRecord>());
        }
    }
}
