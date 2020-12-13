namespace ThAmCo.Catalogue.Services.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class StockManagementService : IStockManagementService
    {

        private readonly HttpClient httpClient;

        public StockManagementService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public IEnumerable<ProductStockModel> GetProductsStock() => throw new NotImplementedException();

        public ProductStockModel GetProductStock(Guid id) => throw new NotImplementedException();

        public ProductStockModel GetProductStock(ProductModel model) => throw new NotImplementedException();

    }
}
