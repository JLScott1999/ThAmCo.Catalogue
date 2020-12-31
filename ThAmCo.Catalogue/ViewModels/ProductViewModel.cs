namespace ThAmCo.Catalogue.ViewModels
{
    using System;
    using System.Collections.Generic;
    using ThAmCo.Catalogue.Models;

    public class ProductViewModel
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? Stock { get; set; }

        public IEnumerable<ProductReviewModel> Reviews { get; set; }

        public string StockStatus
        {
            get
            {
                return this.Stock.HasValue ? this.Stock > 0 ? "In Stock" : "Out of Stock" : "Unknown";
            }
        }

        public DateTime? LastOrdered { get; set; } = null;

    }
}
