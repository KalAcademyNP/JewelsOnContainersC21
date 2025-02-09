using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebMVC.Infrastructure;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly string _baseUrl;
        private readonly IHttpClient _httpClient;
        public CatalogService(IConfiguration config, IHttpClient client) 
        {
            _baseUrl = $"{config["CatalogUrl"]}/api/catalog";
            _httpClient = client ;
        }
        public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
        {
            var brandUri = APIPaths.Catalog.GetAllBrands(_baseUrl);
            var dataString = await _httpClient.GetStringAsync(brandUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var brands = JArray.Parse(dataString);
            foreach (var item in brands)
            {
                items.Add(new SelectListItem
                {
                    Value = item.Value<string>("id"),
                    Text = item.Value<string>("brand")
                });
            }
            return items;
        }

        public async Task<Catalog> GetCatalogItemsAsync(int page, int size, int? brand, int? type)
        {
            var catalogItemsUri = APIPaths.Catalog.GetAllCatalogItems(_baseUrl, 
                page, size, brand, type);
            var dataString = await _httpClient.GetStringAsync(catalogItemsUri);
            return JsonConvert.DeserializeObject<Catalog>(dataString);
        }

        public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
        {
            var typeUri = APIPaths.Catalog.GetAllTypes(_baseUrl);
            var dataString = await _httpClient.GetStringAsync(typeUri);
            var items = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = null,
                    Text = "All",
                    Selected = true
                }
            };

            var types = JArray.Parse(dataString);
            foreach (var item in types)
            {
                items.Add(new SelectListItem
                {
                    Value = item.Value<string>("id"),
                    Text = item.Value<string>("type")
                });
            }
            return items;
        }
    }
}
