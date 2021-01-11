namespace ThAmCo.Catalogue.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class OrderService : IOrderService
    {

        private readonly HttpClient httpClient;

        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductOrderModel>> HasOrderedAsync(Guid id)
        {
            try
            {
                HttpResponseMessage response = this.httpClient.GetAsync("product/ordered/" + id.ToString()).Result;
                response.EnsureSuccessStatusCode();

                if (response.Content is object && response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    IEnumerable<ProductOrderModel> orderList = await JsonSerializer.DeserializeAsync<IEnumerable<ProductOrderModel>>(
                        await response.Content.ReadAsStreamAsync(),
                        new JsonSerializerOptions
                        {
                            IgnoreNullValues = true,
                            PropertyNameCaseInsensitive = true
                        }
                    );
                    return orderList.Where(x => !(x == null || x.ProductId.Equals(Guid.Empty) || x.OrderDate.Equals(DateTime.MinValue)))
                                    .OrderByDescending(x => x.OrderDate);
                }
            }
            catch (Exception)
            {
                // Unknown exception
            }
            return new List<ProductOrderModel>();
        }

    }
}
