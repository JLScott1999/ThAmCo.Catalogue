namespace ThAmCo.Catalogue.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class OrderService : IOrderService
    {

        private readonly HttpClient httpClient;

        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<ProductOrderModel> HasOrdered(ProductModel product) => throw new NotImplementedException();

        public IEnumerable<ProductOrderModel> HasOrdered(Guid productId) => throw new NotImplementedException();

    }
}
