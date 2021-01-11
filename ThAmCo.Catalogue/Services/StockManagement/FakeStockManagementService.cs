namespace ThAmCo.Catalogue.Services.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class FakeStockManagementService : IStockManagementService
    {

        private readonly IEnumerable<ProductStockModel> productStockData;

        public FakeStockManagementService(IEnumerable<ProductStockModel> productStockData)
        {
            this.productStockData = productStockData;
        }

        public FakeStockManagementService() : 
            this(
                new List<ProductStockModel>()
                {
                    new ProductStockModel()
                    {
                        Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        Stock = 10
                    },
                    new ProductStockModel()
                    {
                        Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                        Stock = 0
                    }
                }
            )
        {
        }

        public Task<IEnumerable<ProductStockModel>> GetProductsStockAsync() => Task.FromResult(this.productStockData);

        public Task<ProductStockModel> GetProductStockAsync(Guid id) => Task.FromResult(this.productStockData.FirstOrDefault(x => x.Id.Equals(id)));

    }
}
