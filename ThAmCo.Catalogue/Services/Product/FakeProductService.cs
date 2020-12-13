namespace ThAmCo.Catalogue.Services.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class FakeProductService : IProductService
    {

        private readonly IList<ProductModel> productData;

        public FakeProductService(IList<ProductModel> productData)
        {
            this.productData = productData;
        }

        public FakeProductService() :
            this(
                new List<ProductModel>()
                {
                    new ProductModel()
                    {
                        Id = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        Name = "Test",
                        Description = "TestDesc"
                    },
                    new ProductModel()
                    {
                        Id = Guid.Parse("12E74E96-F987-4B1D-9870-74C84A0A8965"),
                        Name = "TestTwo",
                        Description = "TestTwoDesc"
                    }
                }
            )
        {
        }

        public ProductModel GetProduct(Guid id) => this.productData.FirstOrDefault(p => p.Id.Equals(id));

        public IEnumerable<ProductModel> GetProducts() => this.productData;

        public IEnumerable<ProductModel> SearchProducts(string query)
        {
            query = query.ToLower();
            return this.productData.Where(p => p.Name.ToLower().Contains(query) || p.Description.ToLower().Contains(query));
        }
    }
}
