using KoiVetenary.Business;
using KoiVetenary.Common;
using KoiVetenary.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KoiVetenary.MVCWebApp.Controllers;
public class PaymentAjaxController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var owners = await GetOwners();
        ViewData["OwnerId"] = new SelectList(owners, "OwnerId", "OwnerId", owners.FirstOrDefault()?.OwnerId);
        return View();
    }

    public IActionResult Details(int? id)
    {
        ViewData["paymentId"] = id;
        return View();
    }

    private static async Task<List<Owner>> GetOwners()
    {
        var listOwners = new List<Owner>();
        //
        using (var httpClient = new HttpClient())
        {
            using (var response = await httpClient.GetAsync(Const.API_Endpoint + "Owners"))
            {
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var owners = JsonConvert.DeserializeObject<KoiVetenaryResult>(apiResponse);

                    if (owners != null && owners.Data != null)
                    {
                        listOwners = JsonConvert.DeserializeObject<List<Owner>>(owners.Data.ToString());
                    }
                }
            }
        }
        return listOwners;
    }

}
