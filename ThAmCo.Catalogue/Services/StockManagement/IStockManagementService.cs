namespace ThAmCo.Catalogue.Services.StockManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public interface IStockManagementService : IService
    {

        public IEnumerable<ProductStockModel> GetProductsStock();

        public ProductStockModel GetProductStock(Guid id);

        public ProductStockModel GetProductStock(ProductModel model);

    }
}
