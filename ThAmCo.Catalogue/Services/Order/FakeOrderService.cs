namespace ThAmCo.Catalogue.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public class FakeOrderService : IOrderService
    {
        private readonly IList<ProductOrderModel> productOrderData;

        public FakeOrderService(IList<ProductOrderModel> productOrderData)
        {
            this.productOrderData = productOrderData;
        }

        public FakeOrderService() :
            this(
                new List<ProductOrderModel>()
                {
                    new ProductOrderModel()
                    {
                        ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        DateTime = DateTime.UtcNow.AddDays(-14)
                    },
                    new ProductOrderModel()
                    {
                        ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        DateTime = DateTime.UtcNow.AddDays(-7)
                    }
                }
            )
        {
        }

        public IEnumerable<ProductOrderModel> HasOrdered(ProductModel product) => this.productOrderData.Where(x => x.ProductId.Equals(product.Id));

        public IEnumerable<ProductOrderModel> HasOrdered(Guid productId) => this.productOrderData.Where(x => x.ProductId.Equals(productId));

    }
}
