
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Web.Models;
using NZWalks.Web.Models.DTO;

namespace NZWalks.Web.Controllers
{
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory clientFactory;

        public RegionsController(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionDto> regions = new List<RegionDto>();
            try
            {
                //Get all regions from web api
                var client = clientFactory.CreateClient("NZWalksAPI");

                var responseMessage = await client.GetAsync("https://localhost:7129/regions");

                responseMessage.EnsureSuccessStatusCode();

                regions.AddRange(await responseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {

                throw;
            }


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var client = clientFactory.CreateClient("NZWalksAPI");

                    var httpRequestMessage = new HttpRequestMessage()
                    {
                        RequestUri = new Uri("https://localhost:7129/regions"),
                        Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Post
                    };

                    var responseMessage = await client.SendAsync(httpRequestMessage);

                    responseMessage.EnsureSuccessStatusCode();

                    var response = await responseMessage.Content.ReadFromJsonAsync<RegionDto>();

                    if (response != null)
                    {
                        return RedirectToAction("Index", "Regions");
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(model);
        }
    }
}