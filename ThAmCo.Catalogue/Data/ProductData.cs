namespace ThAmCo.Catalogue.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductData
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string EAN { get; set; }

        public string BrandName { get; set; }

        public double Price { get; set; }

    }
}
