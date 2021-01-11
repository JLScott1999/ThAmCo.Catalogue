namespace ThAmCo.Catalogue.Services.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public interface IStockManagementService : IService
    {

        public Task<IEnumerable<ProductStockModel>> GetProductsStockAsync();

        public Task<ProductStockModel> GetProductStockAsync(Guid id);

    }
}
