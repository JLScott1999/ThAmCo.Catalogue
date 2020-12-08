namespace ThAmCo.Catalogue.Services.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public interface IProductService : IService
    {

        public ProductModel GetProduct(Guid id);

        public IEnumerable<ProductModel> GetProducts();

        public IEnumerable<ProductModel> SearchProducts(string query);

    }
}
