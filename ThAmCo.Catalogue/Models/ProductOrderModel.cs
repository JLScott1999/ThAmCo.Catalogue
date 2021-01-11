namespace ThAmCo.Catalogue.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductOrderModel
    {

        public Guid ProductId { get; set; }

        public DateTime OrderDate { get; set; }

    }
}
