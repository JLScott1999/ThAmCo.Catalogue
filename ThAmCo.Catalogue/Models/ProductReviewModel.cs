namespace ThAmCo.Catalogue.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductReviewModel
    {

        public Guid ProductId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

    }
}
