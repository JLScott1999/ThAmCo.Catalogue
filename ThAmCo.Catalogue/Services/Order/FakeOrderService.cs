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
                        OrderDate = DateTime.UtcNow.AddDays(-14)
                    },
                    new ProductOrderModel()
                    {
                        ProductId = Guid.Parse("14D486C4-CEE6-4C26-B274-CC0E300B0B99"),
                        OrderDate = DateTime.UtcNow.AddDays(-7)
                    }
                }
            )
        {
        }

        public Task<IEnumerable<ProductOrderModel>> HasOrderedAsync(Guid id) => Task.FromResult(this.productOrderData.Where(x => x.ProductId.Equals(id)));
    }
}
