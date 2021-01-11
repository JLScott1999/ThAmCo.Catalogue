namespace ThAmCo.Catalogue.Services.Order
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ThAmCo.Catalogue.Models;

    public interface IOrderService : IService
    {

        public Task<IEnumerable<ProductOrderModel>> HasOrderedAsync(Guid id);

    }
}
