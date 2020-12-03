namespace ThAmCo.Catalogue.Repositories.Product
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class FakeProductRepository : IProductRepository
    {

        private readonly IList<ProductModel> productData;

        public FakeProductRepository(IList<ProductModel> productData)
        {
            this.productData = productData;
        }

        public FakeProductRepository() :
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

        public IEnumerable<ProductModel> Get() => this.productData;

        public ProductModel Get(Guid id) => this.productData.FirstOrDefault(p => p.Id.Equals(id));

    }
}
