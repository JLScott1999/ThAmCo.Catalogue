namespace ThAmCo.Catalogue.Models
{
    using System;

    public class ProductModel
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string EAN { get; set; }

        public string BrandName { get; set; }

        public double Price { get; set; }

    }
}
