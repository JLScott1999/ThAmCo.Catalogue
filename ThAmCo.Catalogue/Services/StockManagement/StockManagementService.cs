namespace ThAmCo.Catalogue.Services.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class StockManagementService : IStockManagementService
    {

        private readonly HttpClient httpClient;

        public StockManagementService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductStockModel>> GetProductsStockAsync()
        {
            try
            {
                HttpResponseMessage response = this.httpClient.GetAsync("product/stock").Result;
                response.EnsureSuccessStatusCode();

                if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    IEnumerable<ProductStockModel> productStockList = await JsonSerializer.DeserializeAsync<IEnumerable<ProductStockModel>>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            IgnoreNullValues = true,
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return productStockList.Where(x => !(x == null || x.Id.Equals(Guid.Empty)));
                }
            }
            catch (Exception)
            {
                // Unknown exception
            }
            return new List<ProductStockModel>();
        }

        public async Task<ProductStockModel> GetProductStockAsync(Guid id)
        {
            try
            {
                HttpResponseMessage response = this.httpClient.GetAsync("product/stock/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    ProductStockModel productStock = await JsonSerializer.DeserializeAsync<ProductStockModel>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            IgnoreNullValues = true,
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return productStock;
                }
            }
            catch (Exception)
            {
                // Unknown exception
            }
            return null;
        }

    }
}
