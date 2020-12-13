namespace ThAmCo.Catalogue.Services.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;
    using ThAmCo.Catalogue.Repositories.Product;

    public class ProductService : IProductService
    {

        private readonly IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public ProductModel GetProduct(Guid id) => this.repository.Get(id);

        public IEnumerable<ProductModel> GetProducts() => this.repository.Get();

        public IEnumerable<ProductModel> SearchProducts(string query)
        {
            query = query.ToLower();
            return this.repository.Get().Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query));
        }

    }
}
