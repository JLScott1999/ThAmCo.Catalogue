namespace ThAmCo.Catalogue.Repositories.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Data;
    using ThAmCo.Catalogue.Models;

    public class ProductRepository : IProductRepository
    {
        private readonly DataContext context;

        public ProductRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<ProductModel> Get() =>
            this.context.ProductData
                .Select(x =>
                    this.DataToModel(x)
                );

        public ProductModel Get(Guid id) =>
            this.context.ProductData
                .Where(x => x.Id.Equals(id))
                .Select(x =>
                    this.DataToModel(x)
                )
                .FirstOrDefault();

        private ProductModel DataToModel(ProductData product)
        {
            return new ProductModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description
            };
        }

    }
}
